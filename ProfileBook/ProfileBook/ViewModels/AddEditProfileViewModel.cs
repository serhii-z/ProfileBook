using Acr.UserDialogs;
using Prism.Navigation;
using Prism.Services;
using ProfileBook.Servises.Authorization;
using ProfileBook.Servises.Profile;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using ProfileBook.Properties;
using ProfileBook.Models;
using ProfileBook.Services.Profile;

namespace ProfileBook.ViewModels
{
    public class AddEditProfileViewModel : BaseViewModel
    {
        private ProfileModel _profile;
        private IAuthorizationService _authorizationService;
        private IProfileService _profileService;
        private IPageDialogService _pageDialog;
        private IProfileImageService _profileImageService;

        public AddEditProfileViewModel(INavigationService navigationService, IAuthorizationService authorizationService, 
            IProfileService profileService, IPageDialogService pageDialog, IProfileImageService profileImageService) :
            base(navigationService)
        {
            _authorizationService = authorizationService;
            _profileService = profileService;
            _pageDialog = pageDialog;
            _profileImageService = profileImageService;
        }

        #region --- Public Properties ---

        public ICommand SaveTapCommand => new Command(OnSaveTap);
        public ICommand ImageTapCommand => new Command(OnImageTap);


        private ImageSource _profileImage = "profile.png";
        public ImageSource ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        private string _entryNickNameText;
        public string EntryNickNameText
        {
            get => _entryNickNameText;
            set => SetProperty(ref _entryNickNameText, value);
        }

        private string _entryNameText;
        public string EntryNameText
        {
            get => _entryNameText;
            set => SetProperty(ref _entryNameText, value);
        }

        private string _editorText;
        public string EditorText
        {
            get => _editorText;
            set => SetProperty(ref _editorText, value);
        }

        #endregion

        #region --- Private Methods ---

        private string ExtractPath()
        {
            var path = _profileImage.ToString();
            path = path.Substring(6);
            return path;
        }

        private void CreateProfile()
        {
            var userId = _authorizationService.GetAuthorizedUserId();
            var path = ExtractPath();

            _profile = new Models.ProfileModel()
            {
                ImagePath = path,
                NickName = _entryNickNameText,
                Name = _entryNameText,
                Description = _editorText,
                CreationTime = DateTime.Now,
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
            _profile.CreationTime = DateTime.Now;
        }

        private void ShowDataProfile()
        {
            ProfileImage = _profile.ImagePath;
            EntryNickNameText = _profile.NickName;
            EntryNameText = _profile.Name;
            EditorText = _profile.Description;
        }

        private async void ReplaceFromGalary()
        {
            var photoPath = await _profileImageService.GetPathFromGalary();

            if(!string.IsNullOrEmpty(photoPath))
            {
                ProfileImage = ImageSource.FromFile(photoPath);
            }
        }

        private async void ReplaceFromCamera()
        {
            var photoPath = await _profileImageService.GetPathAfterCamera();

            if (string.IsNullOrEmpty(photoPath))
            {
                await _pageDialog.DisplayAlertAsync(Resource.AlertTitle, Resource.AddEditAlert, "OK");
            }
            else
            {
                ProfileImage = ImageSource.FromFile(photoPath);
            }
        }

        private void ReplaceWithDefault()
        {
            ProfileImage = "profile.png";
        }

        private void SaveOrUpdate()
        {
            if (_profile == null)
            {
                CreateProfile();
                _profileService.AddProfile(_profile);
            }
            else
            {
                UpdateProfile();
                _profileService.UpdateProfile(_profile);
            }
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSaveTap()
        {
            SaveOrUpdate();

            if (!string.IsNullOrEmpty(_entryNickNameText) && !string.IsNullOrEmpty(_entryNameText))
                await navigationService.GoBackAsync();
        }

        private void OnImageTap()
        {
           UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                        .SetTitle(Resource.AddEditDialog)
                        .Add(Resource.AddEditGalery, ReplaceFromGalary)
                        .Add(Resource.AddEditCamera, ReplaceFromCamera)
                        .Add(Resource.AddEditDelete, ReplaceWithDefault));
        }

        #endregion

        #region --- Overrides ---

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("profile", out ProfileModel profile))
            {
                _profile = profile;

                if(_profile != null)
                {
                    ShowDataProfile();
                }                 
            }
        }

        #endregion
    }
}
