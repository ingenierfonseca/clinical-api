
namespace MedicalSuiteNova.Domain.Dto
{
    public class CurrencyDto
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public string? Code { get; set; }
        public required string Symbol { get; set; }
    }
}
