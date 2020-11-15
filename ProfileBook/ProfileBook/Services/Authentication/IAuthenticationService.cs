﻿using ProfileBook.Models;
using ProfileBook.Servises.Repository;

namespace ProfileBook.Servises.Authentication
{
    public interface IAuthenticationService
    {
        int VerifyUser(IRepository repository, string login, string password);
        bool CheckLogin(IRepository repository, string login);
    }
}
