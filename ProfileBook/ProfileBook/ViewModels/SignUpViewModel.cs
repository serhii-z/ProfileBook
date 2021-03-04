using Prism.Navigation;
using Prism.Services;
using ProfileBook.Models;
using ProfileBook.Servises.Authentication;
using ProfileBook.Servises.Authorization;
using ProfileBook.Validators;
using System.Windows.Input;
using Xamarin.Forms;
using ProfileBook.Properties;

namespace ProfileBook.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private IAuthorizationService _authorizationService;
        private IAuthenticationService _authenticationService;
        private IValidator _validator;
        private IPageDialogService _pageDialog;

        public SignUpViewModel(INavigationService navigationService, IAuthorizationService authorizationService, 
            IAuthenticationService authenticationService, IValidator validator, IPageDialogService pageDialog) :
            base(navigationService)
        {
            _authorizationService = authorizationService;
            _authenticationService = authenticationService;
            _validator = validator;
            _pageDialog = pageDialog;
        }

        #region --- Public Properties ---

        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

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

        private string _entryConfitmPasswordText;
        public string EntryConfirmPasswordText
        {
            get => _entryConfitmPasswordText;
            set
            {
                SetProperty(ref _entryConfitmPasswordText, value);
                CheckTextInput(_entryConfitmPasswordText);             
            }
        }

        private bool _enabledButton = false;
        public bool EnabledButton
        {
            get => _enabledButton;
            set => SetProperty(ref _enabledButton, value);
        }

        #endregion

        #region ---Private Methods ---

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
                !string.IsNullOrEmpty(_entryPasswordText) &&
                !string.IsNullOrEmpty(_entryConfitmPasswordText))
                EnabledButton = true;
        }

        private void MakeButtonInActive()
        {
            EnabledButton = false;
        }

        private bool IsPassValidation()
        {
            if (!_validator.IsQuantityCorrect(_entryLoginText, 4))
            {
                ShowAlert(Resource.ValidatorNumberLogin);
                ClearEntries();
                return false;
            }
            if (_validator.IsFirstSimbolDigit(_entryLoginText))
            {
                ShowAlert(Resource.ValidatorFirst);
                ClearEntries();
                return false;
            }
            if (!_validator.IsQuantityCorrect(_entryPasswordText, 8))
            {
                ShowAlert(Resource.ValidatorNumberPassword);
                ClearEntries();
                return false;
            }
            if (!_validator.IsAvailability(_entryPasswordText))
            {
                ShowAlert(Resource.ValidatorMust);
                ClearEntries();
                return false;
            }
            if (!_validator.IsPasswordsEqual(_entryPasswordText, _entryConfitmPasswordText))
            {
                ShowAlert(Resource.ValidatorPassword);
                ClearEntries();
                return false;
            }

            return true;
        }

        private async void ShowAlert(string message)
        {
            await _pageDialog.DisplayAlertAsync(Resource.AlertTitle, message, "OK");
        }

        private UserModel CreateUser()
        {
            var user = new UserModel()
            {
                Login = _entryLoginText,
                Password = _entryPasswordText
            };

            return user;
        }

        private void ClearEntries()
        {
            EntryLoginText = string.Empty;
            EntryPasswordText = string.Empty;
            EntryConfirmPasswordText = string.Empty;
        }

        private UserModel AddUser()
        {
            UserModel user = null;

            if (IsPassValidation())
            {
                var isBusy = _authenticationService.IsLogin(_entryLoginText);

                if (isBusy)
                {
                    ShowAlert(Resource.AuthorizationAlert);
                    ClearEntries();
                }
                else
                {
                    user = CreateUser();
                    _authenticationService.AddUser(user);
                }
            }

            return user;
        }

        #endregion

        #region ---Private Helpers ---

        private async void OnSignUpTap()
        {
            var user = AddUser();

            if(user != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("login", user.Login);

                await navigationService.GoBackAsync(parameters);
            }
        }

        #endregion
    }
}
