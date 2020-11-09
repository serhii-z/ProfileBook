using ProfileBook.Servises.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Profile
{
    public interface IProfileService
    {
        Task<string> GetPathFromGalary();
        Task<string> GetPathAfterCamera();
        int SaveProfile(IRepository repository, Models.Profile profile);
        int UpdateProfile(IRepository repository, Models.Profile profile);
        int DeleteProfile(IRepository repository, Models.Profile profile);
        List<Models.Profile> GetProfiles(IRepository repository, int userId);     
    }
}
