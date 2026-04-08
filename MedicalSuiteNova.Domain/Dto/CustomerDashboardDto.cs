using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Dto
{
    public class CustomerDashboardDto
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Trend {  get; set; }
        public string Change {  get; set; }
        public string Description { get; set; }
    }
}
