using System.Threading.Tasks;

namespace ProfileBook.Services.Profile
{
    public interface IProfileImageService
    {
        Task<string> GetPathFromGalary();
        Task<string> GetPathAfterCamera();
    }
}
