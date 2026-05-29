
namespace MedicalSuiteNova.Domain.Dto
{
    public class TreatmentCategoryDto
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
