
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class TreatmentPlanTemplateItem
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public required string Name { get; set; }
        public byte Order {  get; set; }

        [ForeignKey("TemplateId")]
        public virtual TreatmentPlanTemplate? Plan { get; set; }
    }
}
