
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class CustomerAccountLedger: IEntity
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public required string TransactionType { get; set; }
        public long ReferenceId { get; set; }
        public required string ReferenceTable { get; set; }
        public decimal Amount { get; set; }
        public byte CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal BalanceAfter { get; set; }
        public required string Description {  get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CreatedBy { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Currency? Currency { get; set; }

        public object GetId() => Id;
    }
}
