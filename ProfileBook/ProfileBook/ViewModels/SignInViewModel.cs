using Prism.Navigation;
using Prism.Services;
using ProfileBook.Servises.Authentication;
using ProfileBook.Servises.Authorization;
using ProfileBook.Servises.Profile;
using ProfileBook.Servises.Repository;
using ProfileBook.Servises.Settings;
using ProfileBook.Validators;
using ProfileBook.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        public SignInViewModel(INavigationService navigationService, IRepository repository, 
            ISettingsManager manager, IAuthorizationService authorization, 
            IAuthenticationService authentication, IValidator validator, 
            IProfileService profileService, IPageDialogService pageDialog) :
            base(navigationService, repository, manager, authorization, authentication, validator, profileService, pageDialog)
        {
        }

        private string _entryLoginText;
        public string EntryLoginText
        {
            get => _entryLoginText;
            set
            {
                SetProperty(ref _entryLoginText, value);
                CheckTextInput(_entryLoginText);                   
            }
        }

        private string _entryPasswordText;
        public string EntryPasswordText
        {
            get => _entryPasswordText;
            set
            {
                SetProperty(ref _entryPasswordText, value);
                CheckTextInput(_entryPasswordText);                
            }
        }

        private bool _enabledButton = false;
        public bool EnabledButton
        {
            get => _enabledButton;
            set => SetProperty(ref _enabledButton, value);
        }

        private void InitializeSettings()
        {
            var themeName = manager.GetThemeName(repository);
            manager.AplyTheme(themeName);
        }

        private void CheckTextInput(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                MakeButtonInActive();
            }
            else
            {
                MakeButtonActive();
            }
        }

        private void MakeButtonActive()
        {
            if (!string.IsNullOrEmpty(_entryLoginText) &&
                !string.IsNullOrEmpty(_entryPasswordText))
                EnabledButton = true;
        }

        private void MakeButtonInActive()
        {
            EnabledButton = false;
        }

        public ICommand GoToSignUpViewCommand => new Command(GoToSignUpView);

        private async void GoToSignUpView()
        {
            await navigationService.NavigateAsync($"{nameof(SignUpView)}");
        }

        public ICommand LogInCommand => new Command(VerifyUser);

        private void VerifyUser()
        {
            var userId = authentication.VerifyUser(repository, _entryLoginText, _entryPasswordText);
            MakeAuthorization(userId);
        }

        private async void MakeAuthorization(int userId)
        {
            if (userId > 0)
            {
                authorization.ExecuteAuthorization(userId);
                GoToMainListView(userId);
            }
            else
            {
                await pageDialog.DisplayAlertAsync(Properties.Resource.AlertTitle, Properties.Resource.SignInAlert, "OK");
                EntryLoginText = string.Empty;
                EntryPasswordText = string.Empty;
            }
        }

        private async void GoToMainListView(int id)
        {
            manager.AplyCulture(repository);

            await navigationService.NavigateAsync($"{nameof(MainListView)}");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("login", out string login))
            {
                EntryLoginText = login;
            }            
        }

        public override void Initialize(INavigationParameters parameters)
        {
            InitializeSettings();
        }
    }
}
