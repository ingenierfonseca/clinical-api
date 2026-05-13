
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class SessionPlanDetail: IEntity
    {
        public int Id { get; set; }
        public int SessionPlanMasterId { get; set; }
        public int TreatmentPlanTemplateItemId { get; set; }
        public required string Status { get; set; }
        public DateTime CompletedAt { get; set; }
        public virtual SessionPlanMaster? SessionPlanMaster { get; set; }
        public virtual TreatmentPlanTemplateItem? TemplateItem { get; set; }
        public object GetId() => Id;
    }
}
