using Plugin.Media;
using Plugin.Media.Abstractions;
using ProfileBook.Servises.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Profile
{
    public class ProfileService : IProfileService
    {
        public async Task<string> GetPathFromGalary()
        {
            var photoPath = string.Empty;

                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        CompressionQuality = 40,
                        CustomPhotoSize = 35,
                        MaxWidthHeight = 200,
                        PhotoSize = PhotoSize.MaxWidthHeight
                    });

                    if(photo != null)
                    {
                        photoPath = photo.Path;
                    }                
                }

            return photoPath;
        }

        public async Task<string> GetPathAfterCamera()
        {
            var photoPath = string.Empty;

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    CompressionQuality = 40,
                    CustomPhotoSize = 35,
                    MaxWidthHeight = 200,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    DefaultCamera = CameraDevice.Rear,
                    Name = $"{DateTime.Now.ToString("dd/MM/yyyy_hh/mm/ss")}.jpg"
                });

                if(file != null)
                {
                    photoPath = file.Path;
                }
            }

            return photoPath;
        }

        public int SaveProfile(IRepository repository, Models.Profile profile)
        {
            return repository.SaveItem(profile).Result;
        }

        public int UpdateProfile(IRepository repository, Models.Profile profile)
        {
            return repository.UpdateItem(profile).Result;
        }

        public int DeleteProfile(IRepository repository, Models.Profile profile)
        {
            return repository.DeleteItem(profile).Result;
        }

        public List<Models.Profile> GetProfiles(IRepository repository, int userId)
        {
            string sql = $"SELECT * FROM Profiles WHERE UserId='{userId}'";
            return repository.GetListItems<Models.Profile>(sql).Result;
        }
    }
}
