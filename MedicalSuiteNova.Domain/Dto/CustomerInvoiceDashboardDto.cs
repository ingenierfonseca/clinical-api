using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Dto
{
    public class CustomerInvoiceDashboardDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public string? Avatar {  get; set; }
        public int Age { get; set; }
        public decimal Balance { get; set; }
        public DateTime? LastPayment {  get; set; }
        public DateTime? LastVisit { get; set; }
        public int CountPaid { get; set; }
        public int CountPending { get; set; }
        public int CountOverdue { get; set; }
    }
}
