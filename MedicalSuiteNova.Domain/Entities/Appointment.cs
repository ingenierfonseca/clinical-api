using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentTypeId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual AppointmentType AppointmentType { get; set; }
    }
}
