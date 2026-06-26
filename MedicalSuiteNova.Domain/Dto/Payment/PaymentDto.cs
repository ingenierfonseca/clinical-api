namespace MedicalSuiteNova.Domain.Dto.Payment
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public byte CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public string? Memo { get; set; }
        public DateTime Date { get; set; }
        public byte PaymentTypeId { get; set; }
    }
}
