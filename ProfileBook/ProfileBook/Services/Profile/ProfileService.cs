using ProfileBook.Servises.Repository;
using System.Collections.Generic;

namespace ProfileBook.Servises.Profile
{
    public class ProfileService : IProfileService
    {
        public void SaveProfile(IRepository repository, Models.Profile profile)
        {
            repository.SaveItem(profile);
        }

        public void UpdateProfile(IRepository repository, Models.Profile profile)
        {
            repository.UpdateItem(profile);
        }

        public List<Models.Profile> GetProfiles(IRepository repository, int userId)
        {
            string sql = $"SELECT * FROM Profiles WHERE UserId='{userId}'";
            return repository.GetListItems<Models.Profile>(sql).Result;
        }

        public int DeleteProfile(IRepository repository, Models.Profile profile)
        {
            return repository.DeleteItemAsync(profile).Result;
        }
    }
}
