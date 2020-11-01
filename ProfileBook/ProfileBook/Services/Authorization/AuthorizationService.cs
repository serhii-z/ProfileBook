using Prism.Services;
using ProfileBook.Models;
using ProfileBook.Servises.Repository;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        public User GetUser(IRepository repository, int id)
        {
            var user = repository.GetItem<User>(id).Result;
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<int> SaveUser(IRepository repository, User user)
        {
            return await repository.SaveItem(user);
        }

        public void ExecuteAutorization(int id)
        {
            App.UserId = id;
        }
    }
}
