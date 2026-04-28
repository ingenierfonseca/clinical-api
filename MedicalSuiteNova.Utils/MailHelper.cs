
using System.Net.Mail;

namespace MedicalSuiteNova.Utils
{
    public static class MailHelper
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
