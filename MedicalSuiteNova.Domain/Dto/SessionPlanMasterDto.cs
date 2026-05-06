
namespace MedicalSuiteNova.Domain.Dto
{
    public class SessionPlanMasterDto
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
        public virtual List<SessionPlanDetailDto>? Items { get; set; }

        public virtual CurrencyDto? Currency { get; set; }
    }
}
