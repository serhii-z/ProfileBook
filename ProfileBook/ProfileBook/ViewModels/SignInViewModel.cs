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
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        private User _user;
        private List<Profile> _profiles;
        private IPageDialogService _pageDialog;

        public SignInViewModel(INavigationService navigationService, IRepository repository, 
            ISettingsManager manager, IAuthorizationService authorization, 
            IAuthenticationService authentication, IValidator validator, 
            IProfileService profileService, IPageDialogService pageDialog) :
            base(navigationService, repository, manager, authorization, authentication, validator, profileService)
        {
            EnabledButton = false;
            _pageDialog = pageDialog;
        }

        private string _entryLoginText;
        public string EntryLoginText
        {
            get => _entryLoginText;
            set
            {
                SetProperty(ref _entryLoginText, value);
                if (!string.IsNullOrEmpty(_entryLoginText))
                    ProcessTextChanged();
                else
                    ProcessTextChangedEmpty();
            }
        }

        private string _entryPasswordText;
        public string EntryPasswordText
        {
            get => _entryPasswordText;
            set
            {
                SetProperty(ref _entryPasswordText, value);
                if (!string.IsNullOrEmpty(_entryPasswordText))
                    ProcessTextChanged();
                else
                    ProcessTextChangedEmpty();
            }
        }

        private bool _enabledButton = false;
        public bool EnabledButton
        {
            get => _enabledButton;
            set => SetProperty(ref _enabledButton, value);
        }

        private void ProcessTextChanged()
        {
            if (!string.IsNullOrEmpty(_entryLoginText) &&
                !string.IsNullOrEmpty(_entryPasswordText))
                EnabledButton = true;
        }

        private void ProcessTextChangedEmpty()
        {
            EnabledButton = false;
        }

        public ICommand GoToSignUpViewCommand => new Command(GoToSignUpView);

        async void GoToSignUpView()
        {
            await navigationService.NavigateAsync($"{nameof(SignUpView)}");
        }

        public ICommand LogInCommand => new Command(VerifyUser);

        private void VerifyUser()
        {
            _user = authentication.FindUser(repository, _entryLoginText, _entryPasswordText);
            MakeAuthorization();
        }

        private async void MakeAuthorization()
        {
            if (_user != null)
            {
                authorization.ExecuteAutorization(_user.Id);
                _profiles = profileService.GetProfiles(repository, _user.Id);
                GoToMainListView();
            }
            else
            {
                await _pageDialog.DisplayAlertAsync(Properties.Resource.AlertTitle, Properties.Resource.SignInAlert, "OK");
                EntryLoginText = string.Empty;
                EntryPasswordText = string.Empty;
            }
        }

        private async void GoToMainListView()
        {
            var parameters = new NavigationParameters();
            parameters.Add("user", _user);
            parameters.Add("profiles", _profiles);
            await navigationService.NavigateAsync($"{nameof(MainListView)}", parameters);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            manager.ChangeTheme();

            if (parameters.TryGetValue("login", out string login))
            {
                EntryLoginText = login;
            }
        }
    }
}
