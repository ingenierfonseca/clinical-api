
namespace MedicalSuiteNova.Domain.Dto
{
    public class InvoiceItemDto
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceId { get; set; }
        public required string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal LineTotal { get; set; }
        public decimal OriginalPrice { get; set; }
        public byte OriginalCurrencyId { get; set; }
    }
}
