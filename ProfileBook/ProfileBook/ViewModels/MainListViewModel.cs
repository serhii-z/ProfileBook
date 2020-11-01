using Prism.Navigation;
using Prism.Services;
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

namespace ProfileBook.ViewModels
{
    public class MainListViewModel : BaseViewModel
    {
        private User _user;
        private List<Profile> _profiles;

        public MainListViewModel(INavigationService navigationService, IRepository repository, 
            ISettingsManager manager, IAuthorizationService authorization, 
            IAuthenticationService authentication, IValidator validator, 
            IProfileService profileService) :
            base(navigationService, repository, manager, authorization, authentication, validator, profileService)
        {
            
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
                    ShowFromSelectedItem();
                }
            }
        }

        private async void ShowFromSelectedItem()
        {
            var item = _selectedItem as Profile;
            var parameters = new NavigationParameters();
            parameters.Add("profile", item);
            await navigationService.NavigateAsync($"{nameof(ModalView)}", parameters, useModalNavigation: true);
        }

        private void ShowCollection()
        {
            Items = new ObservableCollection<Profile>();

            foreach (var item in _profiles)
            {
                Items.Add(item);
            }
            IsNoProfiles = false;
        }

        public ICommand EditCommand => new Command(GoToEditViewAsync);

        private async void GoToEditViewAsync(object ob)
        {
            var profile = ob as Profile;
            var parameters = new NavigationParameters();
            parameters.Add("user", _user);
            parameters.Add("profile", profile);
            parameters.Add("profiles", _profiles);
            await navigationService.NavigateAsync($"{nameof(AddEditProfileView)}", parameters);
        }

        public ICommand DeleteCommand => new Command(Delete);

        private async void Delete(object ob)
        {
            var result = await App.Current.MainPage.DisplayAlert(Properties.Resource.AlertTitle, Properties.Resource.MainListAlertDelete, 
                Properties.Resource.MainListAlertDeleteYes, Properties.Resource.MainListAlertDeleteNo);
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
            manager.IsDarkTheme = false;
            manager.ChangeTheme();
            GoToSignInView();
        }

        private async void GoToSignInView()
        {
            await navigationService.NavigateAsync($"{nameof(SignInView)}");
        }

        public ICommand GoToSettingsViewCommand => new Command(GoToSettingsView);

        private async void GoToSettingsView()
        {
            var parameters = new NavigationParameters();
            parameters.Add("user", _user);
            parameters.Add("profiles", _profiles);
            await navigationService.NavigateAsync($"{nameof(SettingsView)}");
        }

        public ICommand AddCommand => new Command(GoToAddView);

        private async void GoToAddView()
        {
            var parameters = new NavigationParameters();
            parameters.Add("user", _user);
            parameters.Add("profiles", _profiles);
            await navigationService.NavigateAsync($"{nameof(AddEditProfileView)}", parameters);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("user", out User user))
            {
                _user = user;
            }
            if (parameters.TryGetValue("theme", out bool theme))
            {
                if (theme)
                {
                    manager.IsDarkTheme = true;
                }
                manager.ChangeTheme();                
            }
            if (parameters.TryGetValue("profiles", out List<Profile> profiles))
            {
                _profiles = profiles;
            }
            if (profiles.Count > 0)
            {
                ShowCollection();
            }
            else
            {
                IsNoProfiles = true;
            }
        }
    }
}
