
namespace MedicalSuiteNova.Domain.Dto.Update
{
    public class UpdateTreatmentDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public byte CurrencyId { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; }
    }
}
