using ProfileBook.Models;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Authentication
{
    public interface IAuthenticationService
    {
        int VerifyUser(string login, string password);
        bool IsLogin(string login);
        int AddUser(UserModel user);
    }
}
