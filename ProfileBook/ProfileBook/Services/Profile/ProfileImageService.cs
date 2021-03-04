using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace ProfileBook.Services.Profile
{
    public class ProfileImageService : IProfileImageService
    {
        public async Task<string> GetPathFromGalary()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                var photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small
                });

                if (photo != null)
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
                var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    SaveToAlbum = true
                });

                if (photo != null)
                {
                    return photo.Path;
                }
            }

            return string.Empty;
        }
    }
}
