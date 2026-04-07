using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Dto
{
    public class InvoiceInfoDto
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal Total { get; set; }
        public byte StatusId { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string PaymentTerm { get; set; }
    }
}
