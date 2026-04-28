

namespace MedicalSuiteNova.Domain.Dto.Request
{
    public class RequestInvoiceDto
    {
        public int CustomerId { get; set; }
        public byte CurrencyId { get; set; }
        public byte StatusId { get; set; }
        public byte PaymentTermId { get; set; }
        public DateTime IssueDate { get; set; }
        public required List<RequestInvoiceItemDto> Items { get; set; }
    }
}
