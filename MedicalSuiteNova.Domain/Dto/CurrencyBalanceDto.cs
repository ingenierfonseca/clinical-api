using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Dto
{
    public class CurrencyBalanceDto
    {
        public string Symbol {  get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
    }
}
