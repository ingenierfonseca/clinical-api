
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class SessionPlanDetail
    {
        public long Id { get; set; }
        public long SessionPlanMasterId { get; set; }
        public int TreatmentPlanTemplateItemId { get; set; }
        public required string Status { get; set; }
        public DateTime? CompletedAt { get; set; }

        [ForeignKey("SessionPlanMasterId")]
        public virtual SessionPlanMaster? SessionPlanMaster { get; set; }

        [ForeignKey("TreatmentPlanTemplateItemId")]
        public virtual TreatmentPlanTemplateItem? TemplateItem { get; set; }
    }
}
