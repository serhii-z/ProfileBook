using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
        private IPageDialogService _pageDialog;

        public AddEditProfileViewModel(INavigationService navigationService, IRepository repository, 
            ISettingsManager manager, IAuthorizationService authorization, 
            IAuthenticationService authentication, IValidator validator, 
            IProfileService profileService, IPageDialogService pageDialog) :
            base(navigationService, repository, manager, authorization, authentication, validator, profileService)
        {
            _pageDialog = pageDialog;
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
            var str = _pictupeSource.ToString();
            var path = str.Substring(6);
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

        public ICommand TapCommand => new Command(AddPicture);

        public void AddPicture()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                           .SetTitle("Dialog")
                           .Add("Pick at Galery", () =>
                           {
                               GetPathGalary();
                           }, "collections.png")
                           .Add("Take photo with camera", () =>
                           {
                               GetPathCamera();
                           }, "camera.png")
                       );
        }

        private async void GetPathGalary()
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        CompressionQuality = 40,
                        CustomPhotoSize = 35,
                        MaxWidthHeight = 200,
                        PhotoSize = PhotoSize.MaxWidthHeight         
                    });
                    PictureSource = ImageSource.FromFile(photo.Path);
                }
            }
            catch { }
        }

        private async void GetPathCamera()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Sample",
                    CompressionQuality = 40,
                    CustomPhotoSize = 35,
                    MaxWidthHeight = 200,
                    PhotoSize = PhotoSize.MaxWidthHeight,                   
                    DefaultCamera = CameraDevice.Rear,
                    Name = $"{DateTime.Now.ToString("dd/MM/yyyy_hh/mm/ss")}.jpg"
                });

                if (file == null)
                {
                    await _pageDialog.DisplayAlertAsync(Properties.Resource.AlertTitle, 
                        Properties.Resource.AddEditAlert, "OK");
                }
                else
                {
                    PictureSource = ImageSource.FromFile(file.Path);
                }                
            }
        }

        public ICommand SaveCommand => new Command(SaveProfile);

        private void SaveProfile()
        {
            if (_profile == null)
            {
                Save();
            }
            else
            {
                Update();
            }

            GoToMainListView();
        }

        private void Save()
        {
            CreateProfile();
            profileService.SaveProfile(repository, _profile);
        }

        private void Update()
        {
            UpdateProfile();
            profileService.UpdateProfile(repository, _profile);
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
