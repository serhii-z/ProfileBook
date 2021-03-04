
namespace ProfileBook.Validators
{
    public interface IValidator
    {
        bool IsQuantityCorrect(string item, int minLength);
        bool IsFirstSimbolDigit(string item);
        bool IsAvailability(string item);
        bool IsPasswordsEqual(string password, string confirm);
    }
}
