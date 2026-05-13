
namespace MedicalSuiteNova.Domain.Dto
{
    public class SessionPlanDetailDto
    {
        public int Id { get; set; }
        public int SessionPlanMasterId { get; set; }
        public int TreatmentPlanTemplateItemId { get; set; }
        public required string Status { get; set; }
        public DateTime CompletedAt { get; set; }
        public virtual SessionPlanMasterDto? SessionPlanMaster { get; set; }
        public virtual TreatmentPlanTemplateItemDto? TemplateItem { get; set; }
    }
}
