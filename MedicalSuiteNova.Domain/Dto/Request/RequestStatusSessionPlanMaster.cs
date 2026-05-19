
namespace MedicalSuiteNova.Domain.Dto.Request
{
    public class RequestStatusSessionPlanMaster
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public required string Status { get; set; }
    }
}
