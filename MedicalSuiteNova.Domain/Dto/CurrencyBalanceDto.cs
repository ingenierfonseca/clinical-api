
namespace MedicalSuiteNova.Domain.Dto
{
    public class CurrencyBalanceDto
    {
        public required string Symbol {  get; set; }
        public string? Code { get; set; }
        public decimal Amount { get; set; }
    }
}
