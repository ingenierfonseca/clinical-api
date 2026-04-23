
namespace MedicalSuiteNova.Domain.Dto
{
    public class PaymentTermDto
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DaysToDue { get; set; }
    }
}
