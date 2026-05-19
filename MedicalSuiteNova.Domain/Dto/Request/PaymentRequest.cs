
namespace MedicalSuiteNova.Domain.Dto.Request
{
    public class PaymentRequest
    {
        public int OperationTypeId { get; set; }
        public byte CurrencyId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public byte PaymentTypeId { get; set; }
        public int InvoiceId { get; set; }
    }
}
