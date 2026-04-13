using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Dto
{
    public class InvoiceDto
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
        public byte PaymentTermId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public required string CreatedBy { get; set; }
    }
}
