
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Currency : IEntity
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public string? Code { get; set; }
        public required string Symbol { get; set; }
        //public bool IsActive { get; set; }
        public object GetId() => Id;
    }
}
