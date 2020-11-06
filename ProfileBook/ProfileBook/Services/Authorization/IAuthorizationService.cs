using ProfileBook.Models;
using ProfileBook.Servises.Repository;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Authorization
{
    public interface IAuthorizationService
    {
        User GetUser(IRepository repository, int id);
        int SaveUser(IRepository repository, User user);
        void ExecuteAutorization(int id);
        int GetAutorization();
    }
}
