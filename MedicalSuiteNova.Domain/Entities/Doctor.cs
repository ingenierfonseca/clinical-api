
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Doctor: IEntity
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public byte Age { get; set; }
        public required string Specialist { get; set; }
        public required string Phone { get; set; }

        public object GetId() => Id;
        public string getShortName()
        {
            return $"{FirstName.Split(' ', 2)[0]} {LastName.Split(' ', 2)[0]}";
        }
    }
}
