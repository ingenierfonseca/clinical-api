using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class TreatmentPlanTemplate : IEntity
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public byte CategoryId { get; set; }
        public required string Complexity {  get; set; }
        public int EstimatedDurationMonths { get; set; }
        public decimal BasePrice { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public object GetId() => Id;

        public virtual TreatmentCategory? Category { get; set; }
        public virtual ICollection<TreatmentPlanTemplateItem>? Items { get; set; }
    }
}
