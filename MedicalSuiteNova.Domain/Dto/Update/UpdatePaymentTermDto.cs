
namespace MedicalSuiteNova.Domain.Dto.Update
{
    public class UpdatePaymentTermDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int DaysToDue { get; set; }
    }
}
