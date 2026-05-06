
namespace MedicalSuiteNova.Domain.Dto
{
    public class TreatmentPlanTemplateDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public byte CategoryId { get; set; }
        public required string Complexity { get; set; }
        public int EstimatedDurationMonths { get; set; }
        public decimal BasePrice { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }

        public virtual List<TreatmentPlanTemplateItemDto>? Items { get; set; }
    }
}
