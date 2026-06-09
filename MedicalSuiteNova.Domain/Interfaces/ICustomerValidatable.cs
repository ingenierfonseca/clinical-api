
namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface ICustomerValidatable
    {
        string? Phone { get; }
        string? Email { get; }
        DateOnly BirthDate { get; }
    }
}
