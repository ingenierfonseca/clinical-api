
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Currency : IEntity
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public object GetId() => Id;
    }
}
