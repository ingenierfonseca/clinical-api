
namespace MedicalSuiteNova.Domain.Dto
{
    public class PaymentTermDto
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int DaysToDue { get; set; }
    }
}
