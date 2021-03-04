using ProfileBook.Models;
using System.Collections.Generic;

namespace ProfileBook.Servises.Profile
{
    public interface IProfileService
    {
        int AddProfile(ProfileModel profile);
        int UpdateProfile(ProfileModel profile);
        int DeleteProfile(ProfileModel profile);
        List<ProfileModel> GetAllProfiles(int userId);
        List<ProfileModel> Sort(string sortingName);
    }
}
