using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Dto.Responses
{
    public class ResponseInvoiceDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal Total { get; set; }
        public byte CurrencyId { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public required string CreatedBy { get; set; }
        public required List<InvoiceItemDto> Items { get; set; }

        public static ResponseInvoiceDto ToDto(Invoice invoice)
        {
            return new ResponseInvoiceDto
            {
                Id = invoice.Id,
                CustomerId = invoice.CustomerId,
                Number = invoice.Number,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                SubTotal = invoice.SubTotal,
                TaxTotal = invoice.TaxTotal,
                DiscountTotal = invoice.DiscountTotal,
                Total = invoice.Total,
                CurrencyId = invoice.CurrencyId,
                StatusId = invoice.StatusId,
                CreatedBy = invoice.CreatedBy,
                Items = invoice.Items!.Select(i => new InvoiceItemDto
                {
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Tax = i.TaxAmount,
                    Discount = i.Discount,
                    LineTotal = i.LineTotal,
                }).ToList()
            };
        }
    }
}
