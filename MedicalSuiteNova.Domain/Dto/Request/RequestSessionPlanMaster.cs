
namespace MedicalSuiteNova.Domain.Dto.Request
{
    public class RequestSessionPlanMaster
    {
        public int SessionId { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        /*public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }*/
        public byte CurrencyId { get; set; }
        /*public decimal TotalEstimatedPrice { get; set; }*/
        public string? Comments { get; set; }
        public required List<int> PlansIds { get; set; }
    }
}
