﻿using Plugin.Settings;
using ProfileBook.Models;
using ProfileBook.Servises.Repository;

namespace ProfileBook.Servises.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        public User GetUser(IRepository repository, int id)
        {
            return repository.GetItem<User>(id).Result;
        }

        public int SaveUser(IRepository repository, User user)
        {
            return repository.SaveItem(user).Result;
        }

        public void ExecuteAutorization(int id)
        {
            CrossSettings.Current.AddOrUpdateValue("id", id);
        }

        public int GetAutorization()
        {
            return CrossSettings.Current.GetValueOrDefault("id", 0);
        }
    }
}
