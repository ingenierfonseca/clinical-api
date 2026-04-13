
namespace MedicalSuiteNova.Util
{
    public static class StatHelper
    {
        public static double CalculateTrend(int previousTotal, int currentIncrease)
        {
            if (previousTotal <= 0) return 0;

            // Fórmula: (Nuevos / Total Anterior) * 100
            return Math.Round((double)currentIncrease / previousTotal * 100, 1);
        }
    }
}
