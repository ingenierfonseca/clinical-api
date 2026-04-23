
namespace MedicalSuiteNova.Domain.Dto.Request
{
    public class RequestInvoiceItemDto
    {
        public required string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
    }
}
