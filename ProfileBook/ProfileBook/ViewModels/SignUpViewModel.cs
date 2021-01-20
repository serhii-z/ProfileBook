using Prism.Navigation;
using Prism.Services;
using ProfileBook.Models;
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
    public class SignUpViewModel : BaseViewModel
    {
        public SignUpViewModel(INavigationService navigationService, IRepository repository, 
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

        private bool _enabledButton = false;
        public bool EnabledButton
        {
            get => _enabledButton;
            set => SetProperty(ref _enabledButton, value);
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

        private User CreateUser()
        {
            var user = new User()
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

        private bool IsPassValidation()
        {
            if (!validator.CheckQuantity(_entryLoginText, 4))
            {
                ShowAlert(Properties.Resource.ValidatorNumberLogin);
                ClearEntries();
                return false;
            }
            if (validator.CheckIfFirstDigit(_entryLoginText))
            {
                ShowAlert(Properties.Resource.ValidatorFirst);
                ClearEntries();
                return false;
            }
            if (!validator.CheckQuantity(_entryPasswordText, 8))
            {
                ShowAlert(Properties.Resource.ValidatorNumberPassword);
                ClearEntries();
                return false;
            }
            if (!validator.CheckAvailability(_entryPasswordText))
            {
                ShowAlert(Properties.Resource.ValidatorMust);
                ClearEntries();
                return false;
            }
            if (!validator.ComparePasswords(_entryPasswordText, _entryConfitmPasswordText))
            {
                ShowAlert(Properties.Resource.ValidatorPassword);
                ClearEntries();
                return false;
            }

            return true;
        }

        private async void ShowAlert(string message)
        {
            await pageDialog.DisplayAlertAsync(Properties.Resource.AlertTitle, message, "OK");
        }

        public ICommand SignUpCommand => new Command(SaveUser);

        private void SaveUser()
        {
            if (IsPassValidation())
            {
                var isBusy = authentication.CheckLogin(repository, _entryLoginText);

                if (isBusy)
                {
                    ShowAlert(Properties.Resource.AuthorizationAlert);
                    ClearEntries();
                }
                else
                {
                    var user = CreateUser();
                    authorization.SaveUser(repository, user);
                    GoToSignInView(user);
                }
            }
        }

        private async void GoToSignInView(User user)
        {
            var parameters = new NavigationParameters();
            parameters.Add("login", user.Login);

            await navigationService.GoBackAsync(parameters);
        }
    }
}
