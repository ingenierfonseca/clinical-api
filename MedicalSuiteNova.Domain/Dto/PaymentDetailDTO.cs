using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Dto
{
    public class PaymentDetailDTO
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string PaymentTypeName { get; set; }
    }
}
