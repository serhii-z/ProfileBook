using ProfileBook.Models;
using ProfileBook.Servises.Repository;

namespace ProfileBook.Servises.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private IRepository _repository;

        public AuthenticationService(IRepository repository)
        {
            _repository = repository;
        }

        public int VerifyUser(string login, string password)
        {
            string sql = $"SELECT * FROM Users WHERE Login='{login}' AND Password='{password}'";
            var user = _repository.FindWithQueryAsync<UserModel>(sql).Result;

            if(user == null)
            {
                return 0;
            }

            return user.Id;
        }

        public bool IsLogin(string login)
        {
            string sql = $"SELECT * FROM Users WHERE Login='{login}'";
            var user = _repository.FindWithQueryAsync<UserModel>(sql).Result;

            if (user == null)
            {
                return false;
            }

            return true;
        }

        public int AddUser(UserModel user)
        {
            var result = _repository.InsertAsync(user).Result;

            return result;
        }
    }
}
