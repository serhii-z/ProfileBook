using ProfileBook.Models;
using ProfileBook.Servises.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfileBook.Servises.Profile
{
    public class ProfileService : IProfileService
    {
        private IRepository _repository;
        private List<ProfileModel> _profiles;

        public ProfileService(IRepository repository)
        {
            _repository = repository;
            _profiles = new List<ProfileModel>();
        }


        public int AddProfile(ProfileModel profile)
        {
            var result = _repository.InsertAsync(profile).Result;

            return result;
        }

        public int UpdateProfile(ProfileModel profile)
        {
            var result = _repository.UpdateAsync(profile).Result;

            return result;
        }

        public int DeleteProfile(ProfileModel profile)
        {
            var result = _repository.DeleteAsync(profile).Result;

            return result;
        }

        public List<ProfileModel> GetAllProfiles(int userId)
        {
            string sql = $"SELECT * FROM Profiles WHERE UserId='{userId}'";
            _profiles = _repository.QueryAsync<ProfileModel>(sql).Result;

            return _profiles;
        }

        public List<ProfileModel> Sort(string sortingName)
        {
            switch (sortingName)
            {
                case "Name":
                    return _profiles.OrderBy(x => x.Name).ToList();
                case "NickName":
                    return _profiles.OrderBy(x => x.NickName).ToList();
                case "CreationTime":
                    return _profiles.OrderBy(x => x.CreationTime).ToList();
                default:
                    throw new Exception();
            }
        }
    }
}
