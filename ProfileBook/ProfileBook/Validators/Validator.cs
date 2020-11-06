using System.Text.RegularExpressions;

namespace ProfileBook.Validators
{
    public class Validator : IValidator
    {
        public bool CheckFirstSimbol(string item)
        {
            var hasNumber = new Regex(@"^[0-9]");

            if (hasNumber.IsMatch(item))
            {
                return false;
            }

            return true;
        }

        public bool ComparePasswords(string password, string confirm)
        {
            if (!password.Equals(confirm))
            {
                return false;
            }

            return true;
        }

        public bool CheckSimbolsQuantity(string item, int minLength)
        {
            var pattern = @"^.{" + $"{minLength}" + ",16}$";

            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(item);

            if (matches.Count == 0)
            {
                return false;
            }

            return true;
        }

        public bool CheckSimbols(string item)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (!hasNumber.IsMatch(item) || !hasUpperChar.IsMatch(item) || !hasLowerChar.IsMatch(item))
            {
                return false;
            }

            return true;
        }
    }
}
