
using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Invoice : IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal Total {  get; set; }
        public byte CurrencyId { get; set; }
        public byte StatusId { get; set; }
        public byte PaymentTermId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public required string CreatedBy { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Patient { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency? Currency { get; set; }
        [ForeignKey("StatusId")]
        public virtual InvoiceStatus? InvoiceStatus { get; set; }
        [ForeignKey("PaymentTermId")]
        public virtual PaymentTerm? PaymentTerm { get; set; }
        public virtual ICollection<InvoiceItem>? Items { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = [];
        public object GetId() => Id;
    }
}
