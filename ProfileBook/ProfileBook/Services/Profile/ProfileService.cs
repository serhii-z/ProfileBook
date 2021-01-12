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
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small
                });

                if(photo != null)
                {
                    return photo.Path;
                }                
            }

            return string.Empty;
        }

        public async Task<string> GetPathAfterCamera()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    PhotoSize = PhotoSize.Small,
                    DefaultCamera = CameraDevice.Rear,
                    Name = $"{DateTime.Now.ToString("dd/MM/yyyy_hh/mm/ss")}.jpg"
                });

                if(file != null)
                {
                    return file.Path;
                }
            }

            return string.Empty;
        }

        public int SaveProfile(IRepository repository, Models.Profile profile)
        {
            return repository.InsertItem(profile).Result;
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
            return repository.ChooseItems<Models.Profile>(sql).Result;
        }
    }
}
