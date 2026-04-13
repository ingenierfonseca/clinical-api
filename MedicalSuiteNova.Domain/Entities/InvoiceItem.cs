
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class InvoiceItem : IEntity
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal LineTotal { get; set; }
        public virtual Invoice Invoice { get; set; }
        //public virtual Service Service { get; set; }
        public object GetId() => Id;
    }
}
