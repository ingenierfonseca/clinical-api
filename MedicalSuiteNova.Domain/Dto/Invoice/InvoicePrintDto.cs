
namespace MedicalSuiteNova.Domain.Dto.Invoice
{
    public class InvoicePrintDto
    {
        public required string CompanyName { get; set; }
        public required string CompanyAddress { get; set; }
        public required string CompanyPhone { get; set; }
        public required string CompanyEmail { get; set; }
        public required string CompanyNIT { get; set; }
        public required string CompanyLogoUrl { get; set; }
        public required string CustomerName { get; set; }
        //public required string CustomerAddress { get; set; }
        public required string CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public DateTime Date { get; set; }
        public required string CurrencySymbol { get; set; }
        public required string PaymentTerm { get; set; }
        public required string InvoiceNumber { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public List<InvoiceItemDto>? Items { get; set; }
    }
}
