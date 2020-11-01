using ProfileBook.Servises.Repository;
using System.Collections.Generic;

namespace ProfileBook.Servises.Profile
{
    public interface IProfileService
    {
        void SaveProfile(IRepository repository, Models.Profile profile);
        void UpdateProfile(IRepository repository, Models.Profile profile);
        List<Models.Profile> GetProfiles(IRepository repository, int userId);
        int DeleteProfile(IRepository repository, Models.Profile profile);
    }
}
