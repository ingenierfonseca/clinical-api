using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Entities
{
    public class PaymentTerm
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
