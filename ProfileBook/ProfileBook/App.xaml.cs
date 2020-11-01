using Plugin.Settings;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using ProfileBook.Models;
using ProfileBook.Properties;
using ProfileBook.Servises.Authentication;
using ProfileBook.Servises.Authorization;
using ProfileBook.Servises.Profile;
using ProfileBook.Servises.Repository;
using ProfileBook.Servises.Settings;
using ProfileBook.Validators;
using ProfileBook.ViewModels;
using ProfileBook.Views;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace ProfileBook
{
    public partial class App : PrismApplication
    {
        private User _user;
        private bool _isName;
        private bool _isNickName;
        private bool _isDate;
        private string _culture;
        private bool _isDark;
        private List<Profile> _profiles;

        public static int UserId { get; set; }

        public App()
        {
            InitializeComponent();
        }

        public App(IPlatformInitializer initializer = null) : base(initializer) {}

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            CrossSettings.Current.AddOrUpdateValue("id", UserId);
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainListView, MainListViewModel>();
            containerRegistry.RegisterForNavigation<AddEditProfileView, AddEditProfileViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<ModalView, ModalViewModel>();

            //Services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<IValidator>(Container.Resolve<Validator>());
            containerRegistry.RegisterInstance<IProfileService>(Container.Resolve<ProfileService>());
        }

        protected override void OnInitialized()
        {
            UserId = CrossSettings.Current.GetValueOrDefault("id", 0);
            _isDark = CrossSettings.Current.GetValueOrDefault("theme", false);
            _isName = CrossSettings.Current.GetValueOrDefault("name", false);
            _isNickName = CrossSettings.Current.GetValueOrDefault("nickName", false);
            _isDate = CrossSettings.Current.GetValueOrDefault("date", false);
            _culture = CrossSettings.Current.GetValueOrDefault("culture", "en");

            _user = GetUser();
            _profiles = GetProfiles();

            CultureInfo myCIintl = new CultureInfo(_culture, false);
            Resource.Culture = myCIintl;

            GoToView();
        }

        private async void GoToView()
        {
            if (_user != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("user", _user);
                parameters.Add("theme", _isDark);
                parameters.Add("profiles", _profiles);

                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListView)}", parameters);
            }
            else
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
            }
        }

        private User GetUser()
        {
            if (UserId > 0)
            {
                var repository = Container.Resolve(typeof(Repository));
                return Container.Resolve<AuthorizationService>().GetUser((Repository)repository, UserId);
            }
            return null;
        }

        private List<Profile> GetProfiles()
        {
            var temp = new List<Profile>();
            var repository = Container.Resolve(typeof(Repository));

            if (_isName)
            {
                temp = Container.Resolve<SettingsManager>().SortByName((Repository)repository, UserId);
            }              
            else if (_isNickName)
            {
                temp = Container.Resolve<SettingsManager>().SortByNickName((Repository)repository, UserId);
            }              
            else if (_isDate)
            {
                temp = Container.Resolve<SettingsManager>().SortByDate((Repository)repository, UserId);
            }
            else if(_user != null)
            {
                temp = Container.Resolve<ProfileService>().GetProfiles((Repository)repository, _user.Id);
            }
            
            return temp;
        }
    }
}
