
namespace MedicalSuiteNova.Domain.Dto
{
    public class ExchangeRateDto
    {
        public int Id { get; set; }
        public byte FromCurrencyId { get; set; }
        public byte ToCurrencyId { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateDate { get; set; }
        public bool IsActive { get; set; }
        public string? Source { get; set; }

        public virtual CurrencyDto? FromCurrency { get; set; }
        public virtual CurrencyDto? ToCurrency { get; set; }
    }
}
