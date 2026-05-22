
namespace MedicalSuiteNova.Domain.Dto.Request
{
    public class RequestSessionPlanMaster
    {
        public int SessionId { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public byte CurrencyId { get; set; }
        public byte PaymentTermId { get; set; }
        public bool IsFinanced { get; set; }
        public decimal DownPayment { get; set; }
        public string? Comments { get; set; }
        public required List<int> PlansIds { get; set; }
    }
}
