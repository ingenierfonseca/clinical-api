
namespace MedicalSuiteNova.Domain.Dto.Update
{
    public class UpdatePaymentTermDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DaysToDue { get; set; }
    }
}
