using System.Threading.Tasks;

namespace ProfileBook.Validators
{
    public interface IValidator
    {
        bool CheckQuantity(string item, int minLength);
        bool CheckIfFirstDigit(string item);
        bool CheckAvailability(string item);
        bool ComparePasswords(string password, string confirm);
    }
}
