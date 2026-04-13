
namespace MedicalSuiteNova.Domain.Dto.Request
{
    public class RequestInvoiceDto
    {
        public int CustomerId { get; set; }
        public required string CreatedBy { get; set; }
        public byte CurrencyId { get; set; }
        public byte StatusId { get; set; }
        public byte PaymentTermId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public required List<InvoiceItemDto> Items { get; set; }
    }
}
