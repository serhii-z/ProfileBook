using System.Text.RegularExpressions;

namespace ProfileBook.Validators
{
    public class Validator : IValidator
    {
        public bool CheckIfFirstDigit(string item)
        {
            var hasNumber = new Regex(@"^[0-9]");

            if (hasNumber.IsMatch(item))
            {
                return true;
            }

            return false;
        }

        public bool ComparePasswords(string password, string confirm)
        {
            if (password.Equals(confirm))
            {
                return true;
            }

            return false;
        }

        public bool CheckQuantity(string item, int minLength)
        {
            var pattern = @"^.{" + $"{minLength}" + ",16}$";
            var hasSequence = new Regex(pattern);

            if (hasSequence.IsMatch(item))
            {
                return true;
            }

            return false;
        }

        public bool CheckAvailability(string item)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (hasNumber.IsMatch(item) && hasUpperChar.IsMatch(item) && hasLowerChar.IsMatch(item))
            {
                return true;
            }

            return false;
        }
    }
}
