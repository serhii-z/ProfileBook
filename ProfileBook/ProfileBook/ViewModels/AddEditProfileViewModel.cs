using Acr.UserDialogs;
using Prism.Navigation;
using Prism.Services;
using ProfileBook.Models;
using ProfileBook.Servises.Authentication;
using ProfileBook.Servises.Authorization;
using ProfileBook.Servises.Profile;
using ProfileBook.Servises.Repository;
using ProfileBook.Servises.Settings;
using ProfileBook.Validators;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class AddEditProfileViewModel : BaseViewModel
    {
        private Profile _profile;

        public AddEditProfileViewModel(INavigationService navigationService, IRepository repository, 
            ISettingsManager manager, IAuthorizationService authorization, 
            IAuthenticationService authentication, IValidator validator, 
            IProfileService profileService, IPageDialogService pageDialog) :
            base(navigationService, repository, manager, authorization, authentication, validator, profileService, pageDialog)
        {
        }

        private ImageSource _pictupeSource = "profile.png";
        public ImageSource PictureSource
        {
            get => _pictupeSource;
            set => SetProperty(ref _pictupeSource, value);
        }

        private string _entryNickNameText = string.Empty;
        public string EntryNickNameText
        {
            get => _entryNickNameText;
            set => SetProperty(ref _entryNickNameText, value);
        }

        private string _entryNameText = string.Empty;
        public string EntryNameText
        {
            get => _entryNameText;
            set => SetProperty(ref _entryNameText, value);
        }

        private string _editorText = string.Empty;
        public string EditorText
        {
            get => _editorText;
            set => SetProperty(ref _editorText, value);
        }

        private string ExtractPath()
        {
            var path = _pictupeSource.ToString();
            path = path.Substring(6);
            return path;
        }

        private void CreateProfile()
        {
            var userId = authorization.GetAutorization();
            var path = ExtractPath();

            _profile = new Profile()
            {
                ImagePath = path,
                NickName = _entryNickNameText,
                Name = _entryNameText,
                Description = _editorText,
                StartDate = DateTime.Now,
                UserId = userId
            };
        }

        private void UpdateProfile()
        {
            var path = ExtractPath();
            _profile.ImagePath = path;
            _profile.NickName = _entryNickNameText;
            _profile.Name = _entryNameText;
            _profile.Description = _editorText;
        }

        private void ShowDataProfile()
        {
            PictureSource = _profile.ImagePath;
            EntryNickNameText = _profile.NickName;
            EntryNameText = _profile.Name;
            EditorText = _profile.Description;
        }

        public ICommand TapCommand => new Command(ReplacePicture);

        public void ReplacePicture()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                        .SetTitle(Properties.Resource.AddEditDialog)
                        .Add(Properties.Resource.AddEditGalery, () =>
                        {
                            ReplaceFromGalary();
                        })
                        .Add(Properties.Resource.AddEditCamera, () =>
                        {
                            ReplaceFromCamera();
                        })
                        .Add(Properties.Resource.AddEditDelete, () => 
                        {
                            PictureSource = "profile.png";
                        })
                    );
        }

        private async void ReplaceFromGalary()
        {
            var photoPath = await profileService.GetPathFromGalary();

            if(!photoPath.Equals(string.Empty))
            {
                PictureSource = ImageSource.FromFile(photoPath);
            }
        }

        private async void ReplaceFromCamera()
        {
            var photoPath = await profileService.GetPathAfterCamera();

            if (photoPath.Equals(string.Empty))
            {
                await pageDialog.DisplayAlertAsync(Properties.Resource.AlertTitle,
                    Properties.Resource.AddEditAlert, "OK");
            }
            else
            {
                PictureSource = ImageSource.FromFile(photoPath);
            }
        }

        public ICommand SaveCommand => new Command(SaveOrUpdate);

        private void SaveOrUpdate()
        {
            if (_profile == null)
            {
                CreateProfile();
                profileService.SaveProfile(repository, _profile);
            }
            else
            {
                UpdateProfile();
                profileService.UpdateProfile(repository, _profile);
            }

            GoToMainListView();
        }

        private async void GoToMainListView()
        {
            if (!string.IsNullOrEmpty(_entryNickNameText) && !string.IsNullOrEmpty(_entryNameText))
                await navigationService.GoBackAsync();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("profile", out Profile profile))
            {
                _profile = profile;

                if(_profile != null)
                {
                    ShowDataProfile();
                }                 
            }
        }
    }
}
