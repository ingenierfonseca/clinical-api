
namespace MedicalSuiteNova.Utils
{
    public static class PhoneHelper
    {
        public static bool IsValidPhone(string phone)
        {
            var normalized = NormalizePhone(phone);

            return normalized.Length == 8;
        }

        public static string NormalizePhone(string phone)
        {
            return new string(phone.Where(char.IsDigit).ToArray());
        }
    }
}
