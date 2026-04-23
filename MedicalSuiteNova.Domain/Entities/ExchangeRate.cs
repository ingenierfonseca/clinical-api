
using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class ExchangeRate: IEntity
    { 
        public int Id { get; set; }
        public byte FromCurrencyId { get; set; }
        public byte ToCurrencyId { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateDate { get; set; }
        public bool IsActive { get; set; }
        public string Source {  get; set; }

        [ForeignKey("FromCurrencyId")]
        public virtual Currency FromCurrency { get; set; }
        [ForeignKey("ToCurrencyId")]
        public virtual Currency ToCurrency { get; set; }

        public object GetId() => Id;
    }
}
