namespace MedicalSuiteNova.Domain.Dto.Payment
{
    public class PaymentDetailDTO
    {
        public int Id { get; set; }
        public required string InvoiceNumber { get; set; }
        public required string CurrencySymbol { get; set; }
        public decimal Amount { get; set; }
        public string? Memo { get; set; }
        public DateTime Date { get; set; }
        public required string PaymentTypeName { get; set; }
    }
}
