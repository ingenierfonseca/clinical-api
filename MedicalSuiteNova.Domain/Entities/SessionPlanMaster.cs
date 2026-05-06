
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class SessionPlanMaster: IEntity
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte CurrencyId { get; set; }
        public decimal TotalEstimatedPrice { get; set; }
        public string? Comments { get; set; }
        public virtual ICollection<SessionPlanDetail>? Items { get; set; }

        public virtual Currency? Currency { get; set; }
        public virtual ClinicalSession? ClinicalSession { get; set; }
        public object GetId() => Id;
    }
}
