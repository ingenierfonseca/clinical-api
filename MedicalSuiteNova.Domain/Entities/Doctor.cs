using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Specialist { get; set; }
        public string Phone { get; set; }

        public string getShortName()
        {
            return $"{FirstName.Split(' ', 2)[0]} {LastName.Split(' ', 2)[0]}";
        }
    }
}
