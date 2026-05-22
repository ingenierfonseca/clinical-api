
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Validation
{
    public class SessionPlanValidationContext
    {
        public required ClinicalSession ClinicalSession { get; set; }
        public required Customer Customer { get; set; }
        public required List<TreatmentPlanTemplate> PlanTemplates { get; set; }
    }
}
