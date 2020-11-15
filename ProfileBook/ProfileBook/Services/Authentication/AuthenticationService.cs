using ProfileBook.Models;
using ProfileBook.Servises.Repository;

namespace ProfileBook.Servises.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public int VerifyUser(IRepository repository, string login, string password)
        {
            string sql = $"SELECT * FROM Users WHERE Login='{login}' AND Password='{password}'";
            var user = repository.FindItem<User>(sql).Result;

            if(user != null)
            {
                return user.Id;
            }

            return 0;
        }

        public bool CheckLogin(IRepository repository, string login)
        {
            string sql = $"SELECT * FROM Users WHERE Login='{login}'";
            var user = repository.FindItem<User>(sql).Result;

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}
