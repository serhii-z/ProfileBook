using Plugin.Settings;
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
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private User _user;
        private List<Profile> _profiles;
        public SettingsViewModel(INavigationService navigationService, IRepository repository, 
            ISettingsManager manager, IAuthorizationService authorization, 
            IAuthenticationService authentication, IValidator validator, IProfileService profileService) :
            base(navigationService, repository, manager, authorization, authentication, validator, profileService)
        { 
        
        }

        private bool _isName;
        public bool IsName
        {
            get => _isName;
            set
            {
                SetProperty(ref _isName, value);
                if (_isName)
                {
                    IsNickName = false;
                    IsDate = false;
                    CrossSettings.Current.AddOrUpdateValue("name", true);
                }
                else
                {
                    CrossSettings.Current.AddOrUpdateValue("name", false);
                }
            }
        }

        private bool _isNickName;
        public bool IsNickName
        {
            get => _isNickName;
            set
            {
                SetProperty(ref _isNickName, value);
                if (_isNickName)
                {
                    IsName = false;
                    IsDate = false;
                    CrossSettings.Current.AddOrUpdateValue("nickName", true);
                }
                else
                {
                    CrossSettings.Current.AddOrUpdateValue("nickName", false);
                }
            }
        }

        private bool _isDate;
        public bool IsDate
        {
            get => _isDate;
            set
            {
                SetProperty(ref _isDate, value);
                if (_isDate)
                {
                    IsName = false;
                    IsNickName = false;
                    CrossSettings.Current.AddOrUpdateValue("date", true);
                }
                else
                {
                    CrossSettings.Current.AddOrUpdateValue("date", false);
                }
            }
        }

        private bool _isTheme;
        public bool IsTheme
        {
            get => _isTheme;
            set
            {
                SetProperty(ref _isTheme, value);
                manager.IsDarkTheme = _isTheme;
                manager.ChangeTheme();
                CrossSettings.Current.AddOrUpdateValue("theme", _isTheme);
            }
        }

        private bool _isEnglish;
        public bool IsEnglish
        {
            get => _isEnglish;
            set
            {
                SetProperty(ref _isEnglish, value);
                if (_isEnglish)
                {
                    IsRussian = false;
                    manager.ChengeCulture("en");
                    CrossSettings.Current.AddOrUpdateValue("eng", true);
                    CrossSettings.Current.AddOrUpdateValue("rus", false);
                }
                else
                {
                    manager.ChengeCulture("en");
                    CrossSettings.Current.AddOrUpdateValue("eng", false);
                }
            }
        }

        private bool _isRussian;
        public bool IsRussian
        {
            get => _isRussian;
            set
            {
                SetProperty(ref _isRussian, value);
                if (_isRussian)
                {
                    IsEnglish = false;
                    manager.ChengeCulture("ru");
                    CrossSettings.Current.AddOrUpdateValue("rus", true);
                    CrossSettings.Current.AddOrUpdateValue("eng", false);
                }
                else
                {
                    manager.ChengeCulture("en");
                    CrossSettings.Current.AddOrUpdateValue("rus", false);
                }
            }
        }

        private void SortByName()
        {
            if (_isName)
                _profiles = manager.SortByName(repository, App.UserId);
        }

        private void SortByNickName()
        {
            if (_isNickName)
                _profiles = manager.SortByNickName(repository, App.UserId);
        }

        private void SortByDate()
        {
            if (_isDate)
                _profiles = manager.SortByDate(repository, App.UserId);
        }

        private void GetWithoutSorting()
        {
            if (!_isName && !_isNickName && !_isDate)
                _profiles = profileService.GetProfiles(repository, App.UserId);
        }

        public ICommand GoToMainListCommand => new Command(GotoMainList);

        private async void GotoMainList()
        {
            SortByName();
            SortByNickName();
            SortByDate();
            GetWithoutSorting();

            var parameters = new NavigationParameters();
            parameters.Add("user", _user);
            parameters.Add("profiles", _profiles);
            await navigationService.NavigateAsync($"{nameof(MainListView)}", parameters);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            IsName = CrossSettings.Current.GetValueOrDefault("name", false);
            IsNickName = CrossSettings.Current.GetValueOrDefault("nickName", false);
            IsDate = CrossSettings.Current.GetValueOrDefault("date", false);
            IsEnglish = CrossSettings.Current.GetValueOrDefault("eng", false);
            IsRussian = CrossSettings.Current.GetValueOrDefault("rus", false);
            IsTheme = CrossSettings.Current.GetValueOrDefault("theme", false);

            if (parameters.TryGetValue("user", out User user))
            {
                _user = user;
            }
            if (parameters.TryGetValue("profiles", out List<Profile> profiles))
            {
                if (profiles.Count > 0)
                {
                    _profiles = profiles;
                }
            }
        }
    }
}
