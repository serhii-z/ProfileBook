using ProfileBook.Servises.Repository;
using System.Collections.Generic;

namespace ProfileBook.Servises.Profile
{
    public interface IProfileService
    {
        int SaveProfile(IRepository repository, Models.Profile profile);
        int UpdateProfile(IRepository repository, Models.Profile profile);
        int DeleteProfile(IRepository repository, Models.Profile profile);
        List<Models.Profile> GetProfiles(IRepository repository, int userId);     
    }
}
