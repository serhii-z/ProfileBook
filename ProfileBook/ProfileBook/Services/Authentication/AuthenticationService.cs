using ProfileBook.Models;
using ProfileBook.Servises.Repository;

namespace ProfileBook.Servises.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public int FindUser(IRepository repository, string login, string password)
        {
            string sql = $"SELECT * FROM Users WHERE Login='{login}' AND Password='{password}'";
            var list = repository.GetListItems<User>(sql).Result;

            if(list.Count > 0)
            {
                return list[0].Id;
            }

            return 0;
        }

        public bool CheckLogin(IRepository repository, string login)
        {
            string sql = $"SELECT * FROM Users WHERE Login='{login}'";
            var list = repository.GetListItems<User>(sql).Result;

            if (list.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
