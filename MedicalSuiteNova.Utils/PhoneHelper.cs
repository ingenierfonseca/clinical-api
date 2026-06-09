
using System.Text.RegularExpressions;

namespace MedicalSuiteNova.Utils
{
    public static partial class PhoneHelper
    {
        [GeneratedRegex(@"^\d{8}$")]
        private static partial Regex PhoneRegex();

        public static bool ValidatePhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return true;

            return PhoneRegex().IsMatch(phone);
        }

        public static string NormalizePhone(string phone)
        {
            return new string(phone.Where(char.IsDigit).ToArray());
        }
    }
}
