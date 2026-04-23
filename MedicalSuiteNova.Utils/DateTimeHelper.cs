
namespace MedicalSuiteNova.Utils
{
    public static class DateTimeHelper
    {
        public static (bool isValid, string message) ValidateIssueDate(DateTime issueDate)
        {
            if (issueDate == default)
                return (false, "La fecha de emisión es obligatoria.");

            // 2. No permitir fechas futuras (opcional, según tu regla)
            if (issueDate > DateTime.Now.AddDays(1))
                return (false, "La fecha de emisión no puede ser futura.");

            // 3. No permitir fechas demasiado antiguas (ej. más de 30 días atrás)
            if (issueDate < DateTime.Now.AddDays(-30))
                return (false, "La fecha de emisión es demasiado antigua para el periodo contable actual.");

            return (true, "OK");
        }
    }
}
