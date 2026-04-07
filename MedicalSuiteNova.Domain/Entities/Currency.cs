using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Currency
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
    }
}
