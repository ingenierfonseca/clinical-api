
namespace MedicalSuiteNova.Domain.Dto
{
    public class TreatmentPlanTemplateItemDto
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public required string Name { get; set; }
        public byte Order { get; set; }
    }
}
