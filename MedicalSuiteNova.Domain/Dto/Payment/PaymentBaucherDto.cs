
namespace MedicalSuiteNova.Domain.Dto.Payment
{
    public class PaymentBaucherDto
    {
        public required string CompanyName { get; set; }
        public required string CompanyAddress { get; set; }
        public required string CompanyPhone { get; set; }
        public required string CompanyNIT {  get; set; }
        public required string ReceiptNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public required string CurrencySymbol { get; set; }
        public required string PaymentMethod { get; set; }
        public bool IsPartialPayment { get; set; }
        public string? Memo { get; set; }
        public required string InvoiceNumber { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal RemainingBalance { get; set; }
        public List<InvoiceItemDto>? Items { get; set; }
    }
}
