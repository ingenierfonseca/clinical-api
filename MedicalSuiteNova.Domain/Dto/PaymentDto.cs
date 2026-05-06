
namespace MedicalSuiteNova.Domain.Dto
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public byte PaymentTypeId { get; set; }
    }
}
