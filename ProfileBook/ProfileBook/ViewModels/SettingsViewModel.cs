using Prism.Navigation;
using Prism.Services;
using ProfileBook.Servises.Authentication;
using ProfileBook.Servises.Authorization;
using ProfileBook.Servises.Profile;
using ProfileBook.Servises.Repository;
using ProfileBook.Servises.Settings;
using ProfileBook.Validators;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(INavigationService navigationService, IRepository repository, 
            ISettingsManager manager, IAuthorizationService authorization, 
            IAuthenticationService authentication, IValidator validator, 
            IProfileService profileService, IPageDialogService pageDialog) :
            base(navigationService, repository, manager, authorization, authentication, validator, profileService, pageDialog)
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
                    manager.AddOrUpdateSorting(repository, "name");
                }
                else
                {
                    manager.AddOrUpdateSorting(repository, string.Empty);
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
                    manager.AddOrUpdateSorting(repository, "nickName");
                }
                else
                {
                    manager.AddOrUpdateSorting(repository, string.Empty);
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
                    manager.AddOrUpdateSorting(repository, "date");
                }
                else
                {
                    manager.AddOrUpdateSorting(repository, string.Empty);
                }
            }
        }

        private bool _isDark;
        public bool IsDark
        {
            get => _isDark;
            set
            {
                SetProperty(ref _isDark, value);
                if (_isDark)
                {
                    manager.AddOrUpdateTheme(repository, "dark");
                }
                else
                {
                    manager.AddOrUpdateTheme(repository, string.Empty);
                }             
            }
        }

        private bool _isUkrainian;
        public bool IsUkrainian
        {
            get => _isUkrainian;
            set
            {
                SetProperty(ref _isUkrainian, value);
                if (_isUkrainian)
                {
                    IsRussian = false;
                    manager.AddOrUpdateCulture(repository, "uk");
                }
                else
                {
                    manager.AddOrUpdateCulture(repository, string.Empty);
                }
                manager.AplyCulture(repository);
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
                    IsUkrainian = false;
                    manager.AddOrUpdateCulture(repository, "ru");
                }
                else
                {                  
                    manager.AddOrUpdateCulture(repository, string.Empty);
                }
                manager.AplyCulture(repository);
            }
        }

        private void ActivateSorting()
        {
            string sortingName = manager.GetSortingName(repository);

            switch (sortingName)
            {
                case "name":
                    IsName = true;
                    break;
                case "nickName":
                    IsNickName = true;
                    break;
                case "date":
                    IsDate = true;
                    break;
            }
        }

        private void ActivateTheme()
        {
            string themeName = manager.GetThemeName(repository);

            switch (themeName)
            {
                case "dark":
                    IsDark = true;
                    break;
            }
        }

        private void ActivateCulture()
        {
            string cultureName = manager.GetCultureName(repository);

            switch (cultureName)
            {
                case "uk":
                    IsUkrainian = true;
                    break;
                case "ru":
                    IsRussian = true;
                    break;
            }
        }

        public ICommand GoToMainListCommand => new Command(GotoMainList);

        private async void GotoMainList()
        {
            await navigationService.GoBackAsync();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            ActivateSorting();
            ActivateCulture();
            ActivateTheme();
        }
    }
}
