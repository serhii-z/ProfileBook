using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Servises.Authorization;
using ProfileBook.Servises.Profile;
using ProfileBook.Servises.Settings;
using ProfileBook.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using ProfileBook.Properties;
using Prism.Services;
using ProfileBook.ResourceActivator;
using ProfileBook.Resources.Themes;
using System.ComponentModel;

namespace ProfileBook.ViewModels
{
    public class MainListViewModel : BaseViewModel
    {
        private ISettingsManager _settingsManager;
        private IProfileService _profileService;
        private IAuthorizationService _authorizationService;
        private IPageDialogService _pageDialog;
        private ResourceDictionary _resourceDictionary;
        private IThemeActivator _themeActivator;
        private ICultureActivator _cultureActivator;
        public MainListViewModel(INavigationService navigationService,
            ISettingsManager settingsManager, IAuthorizationService authorizationService,  
            IProfileService profileService, IPageDialogService pageDialog, 
            ResourceDictionary resourceDictionary, IThemeActivator themeActivator,
            ICultureActivator cultureActivator) :
            base(navigationService)
        {
            _settingsManager = settingsManager;
            _profileService = profileService;
            _authorizationService = authorizationService;
            _pageDialog = pageDialog;
            _resourceDictionary = resourceDictionary;
            _themeActivator = themeActivator;
            _cultureActivator = cultureActivator;
        }

        #region --- Public Properties ---

        public ICommand DeleteTapCommand => new Command(OnDeleteTap);
        public ICommand LogOutTapCommand => new Command(OnLogOutTap);
        public ICommand SettingsTapCommand => new Command(OnSettingsTap);
        public ICommand AddTapCommand => new Command(OnAddTap);
        public ICommand EditTapCommand => new Command(OnEditTap);

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ObservableCollection<ProfileModel> _items;
        public ObservableCollection<ProfileModel> Items
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
            set => SetProperty(ref _selectedItem, value);
        }

        #endregion

        #region --- Private Methods ---

        private List<ProfileModel> GetProfiles()
        {
            var sortingName = _settingsManager.SortingName;
            var profiles = _profileService.GetAllProfiles(_authorizationService.GetAuthorizedUserId());

            if (!string.IsNullOrEmpty(sortingName))
            {
                profiles = _profileService.Sort(_settingsManager.SortingName);
            }

            return profiles;
        }

        private void ShowProfiles(List<ProfileModel> profiles)
        {
            if (profiles.Count > 0)
            {
                Items = new ObservableCollection<ProfileModel>();

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

        private void InitSettings()
        {
            _themeActivator.AplyTheme(_resourceDictionary);

            Title = Resource.MainListTitle;
        }

        private void CancelAuthorization()
        {
            _authorizationService.AddOrUpdateAuthorization(0);
            _settingsManager.ThemeName = nameof(LightTheme);
            _themeActivator.AplyTheme(_resourceDictionary);
            _cultureActivator.AplyCulture();
        }

        #endregion

        #region --- Private Helpers ---       

        private async void OnDeleteTap(object ob)
        {
            bool isDialogYes = await _pageDialog.DisplayAlertAsync(Resource.AlertTitle,
                Resource.MainListAlertDelete, Resource.MainListAlertDeleteYes,
                Resource.MainListAlertDeleteNo);

            if (isDialogYes)
            {
                var profile = ob as ProfileModel;
                var answer = _profileService.DeleteProfile(profile);

                if (answer == 1)
                {
                    Items.Remove(profile);
                }

                IsNoProfiles = Items.Count > 0 ? false : true;
            }
        }

        private async void OnLogOutTap()
        {
            CancelAuthorization();

            await navigationService.NavigateAsync($"{nameof(SignInView)}");
        }

        private async void OnSettingsTap()
        {
            await navigationService.NavigateAsync($"{nameof(SettingsView)}");
        }

        private async void OnAddTap()
        {
            await navigationService.NavigateAsync($"{nameof(AddEditProfileView)}");
        }

        private async void OnEditTap(object ob)
        {
            var profile = ob as ProfileModel;
            var parameters = new NavigationParameters();
            parameters.Add("profile", profile);

            await navigationService.NavigateAsync($"{nameof(AddEditProfileView)}", parameters);
        }

        private async void OnSelectedItemTap()
        {
            var item = _selectedItem as ProfileModel;
            var parameters = new NavigationParameters();
            parameters.Add("profile", item);

            await navigationService.NavigateAsync($"{nameof(ModalView)}", parameters, useModalNavigation: true);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedItem))
            {
                OnSelectedItemTap();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            InitSettings();

            var profiles = GetProfiles();
            ShowProfiles(profiles);
        }

        #endregion
    }
}
