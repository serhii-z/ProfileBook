using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Servises.Authentication;
using ProfileBook.Servises.Authorization;
using ProfileBook.Servises.Profile;
using ProfileBook.Servises.Repository;
using ProfileBook.Servises.Settings;
using ProfileBook.Validators;

namespace ProfileBook.ViewModels
{
    public class BaseViewModel : BindableBase, IInitialize, INavigatedAware
    {
        protected readonly INavigationService navigationService;
        protected readonly IRepository repository;
        protected readonly ISettingsManager manager;
        protected readonly IAuthorizationService authorization;
        protected readonly IAuthenticationService authentication;
        protected readonly IValidator validator;
        protected readonly IProfileService profileService;

        public BaseViewModel(INavigationService navigationService, IRepository repository,
            ISettingsManager manager, IAuthorizationService authorization,
            IAuthenticationService authentication, IValidator validator, 
            IProfileService profileService)
        {
            this.navigationService = navigationService;
            this.repository = repository;
            this.manager = manager;
            this.authorization = authorization;
            this.authentication = authentication;
            this.validator = validator;
            this.profileService = profileService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}
