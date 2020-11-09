using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Servises.Authentication;
using ProfileBook.Servises.Authorization;
using ProfileBook.Servises.Profile;
using ProfileBook.Servises.Repository;
using ProfileBook.Servises.Settings;
using ProfileBook.Validators;
using ProfileBook.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using ProfileBook.Properties;
using Prism.Services;

namespace ProfileBook.ViewModels
{
    public class MainListViewModel : BaseViewModel
    {
        public MainListViewModel(INavigationService navigationService, IRepository repository, 
            ISettingsManager manager, IAuthorizationService authorization, 
            IAuthenticationService authentication, IValidator validator, 
            IProfileService profileService, IPageDialogService pageDialog) :
            base(navigationService, repository, manager, authorization, authentication, validator, profileService, pageDialog)
        {            
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ObservableCollection<Profile> _items;
        public ObservableCollection<Profile> Items
        {
            get { return _items; }
            set => SetProperty(ref _items, value);
        }

        private bool _isNoProfiles;
        public bool IsNoProfiles
        {
            get => _isNoProfiles;
            set => SetProperty(ref _isNoProfiles, value);
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                if (_selectedItem == value)
                {
                    GoToModalView();
                }
            }
        }

        private async void GoToModalView()
        {
            var item = _selectedItem as Profile;
            var parameters = new NavigationParameters();
            parameters.Add("profile", item);

            await navigationService.NavigateAsync($"{nameof(ModalView)}", parameters, useModalNavigation: true);
        }

        private List<Profile> GetProfiles(string sortingName, int userId)
        {
            var profiles = profileService.GetProfiles(repository, userId);

            switch (sortingName)
            {
                case "name":
                    profiles = manager.SortByName(repository, userId);
                    break;
                case "nickName":
                    profiles = manager.SortByNickName(repository, userId);
                    break;
                case "date":
                    profiles = manager.SortByDate(repository, userId);
                    break;
            }

            return profiles;
        }

        private void ShowCollection()
        {
            var sortingName = manager.GetSortingName();
            var userId = authorization.GetAutorization();
            var profiles = GetProfiles(sortingName, userId);

            if (profiles.Count > 0)
            {
                Items = new ObservableCollection<Profile>();
                foreach (var item in profiles)
                {
                    Items.Add(item);
                }
                IsNoProfiles = false;
            }
            else
            {
                IsNoProfiles = true;
            }           
        }

        private void InitializeSettings()
        {
            var themeName = manager.GetThemeName();
            manager.ApplyTheme(themeName);

            Title = Resource.MainListTitle;
        }

        public ICommand DeleteCommand => new Command(Delete);

        private async void Delete(object ob)
        {
            var result = await App.Current.MainPage.DisplayAlert(Properties.Resource.AlertTitle, 
                Properties.Resource.MainListAlertDelete, Properties.Resource.MainListAlertDeleteYes, 
                Properties.Resource.MainListAlertDeleteNo);

            if (result)
            {
                var profile = ob as Profile;
                var answer = profileService.DeleteProfile(repository, profile);

                if (answer == 1)
                {
                    Items.Remove(profile);                   
                }

                IsNoProfiles = Items.Count > 0 ? false : true;
            }
        }

        public ICommand LogOutCommand => new Command(CancelAuthorization);

        private void CancelAuthorization()
        {
            authorization.ExecuteAutorization(0);
            manager.ApplyTheme(string.Empty);
            GoToSignInView();
        }

        private async void GoToSignInView()
        {
            await navigationService.NavigateAsync($"{nameof(SignInView)}");
        }

        public ICommand GoToSettingsViewCommand => new Command(GoToSettingsView);

        private async void GoToSettingsView()
        {
            await navigationService.NavigateAsync($"{nameof(SettingsView)}");
        }

        public ICommand EditCommand => new Command(GoToEditViewAsync);

        private async void GoToEditViewAsync(object ob)
        {
            var profile = ob as Profile;
            var parameters = new NavigationParameters();
            parameters.Add("profile", profile);

            await navigationService.NavigateAsync($"{nameof(AddEditProfileView)}", parameters);
        }

        public ICommand AddCommand => new Command(GoToAddView);

        private async void GoToAddView()
        {
            await navigationService.NavigateAsync($"{nameof(AddEditProfileView)}");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            InitializeSettings();
            ShowCollection();
        }
    }
}
