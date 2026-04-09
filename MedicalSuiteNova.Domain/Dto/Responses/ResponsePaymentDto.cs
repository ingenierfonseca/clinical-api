using MedicalSuiteNova.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Dto.Responses
{
    public class ResponsePaymentDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public static ResponsePaymentDto ToDto(Payment payment)
        {
            return new ResponsePaymentDto
            {
                Id = payment.Id,
                CustomerId = payment.CustomerId,
                InvoiceId = payment.InvoiceId,
                Amount = payment.Amount,
                Date = payment.Date
            };
        }
    }
}
