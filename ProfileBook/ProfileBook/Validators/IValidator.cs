using System.Threading.Tasks;

namespace ProfileBook.Validators
{
    public interface IValidator
    {
        bool CheckSimbolsQuantity(string item, int minLength);
        bool CheckFirstSimbol(string item);
        bool CheckSimbols(string item);
        bool ComparePasswords(string password, string confirm);
    }
}
