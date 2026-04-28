
namespace MedicalSuiteNova.Domain.Dto
{
    public class PaymentDetailDTO
    {
        public int Id { get; set; }
        public required string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public required string PaymentTypeName { get; set; }
    }
}
