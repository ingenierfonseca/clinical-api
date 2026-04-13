using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Dto
{
    public class AppointmentInfoDto
    {
        public int Id { get; set; }
        public string PatientName {  get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorName    { get; set; }
        public string TypeName { get; set; }
    }
}
