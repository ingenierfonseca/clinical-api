
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Doctor: IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte Age { get; set; }
        public string Specialist { get; set; }
        public string Phone { get; set; }

        public object GetId() => Id;
        public string getShortName()
        {
            return $"{FirstName.Split(' ', 2)[0]} {LastName.Split(' ', 2)[0]}";
        }
    }
}
