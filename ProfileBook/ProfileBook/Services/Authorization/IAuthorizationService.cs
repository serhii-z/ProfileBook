﻿using ProfileBook.Models;
using ProfileBook.Servises.Repository;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Authorization
{
    public interface IAuthorizationService
    {
        int SaveUser(IRepository repository, User user);
        void ExecuteAuthorization(int id);
        int GetUserId();
    }
}
