
using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class InvoiceItem : IEntity
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceId { get; set; }
        public required string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal OriginalPrice { get; set; }
        public byte OriginalCurrencyId { get; set; }

        public virtual Invoice? Invoice { get; set; }

        [ForeignKey("OriginalCurrencyId")]
        public virtual Currency? Currency { get; set; }
        //public virtual Service Service { get; set; }

        public object GetId() => Id;
    }
}
