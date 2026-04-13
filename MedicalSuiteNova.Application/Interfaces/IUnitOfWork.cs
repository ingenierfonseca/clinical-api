
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPaymentRepository Payments { get; }
        IPaymentTypeRepository PaymentTypes { get; }
        IInvoiceRepository Invoices { get; }
        ICustomerRepository Customers { get; }
        IAppointmentRepository Appointments { get; }
        IAppointmentTypeRepository AppointmentTypes { get; }
        IDoctorRepository Doctors { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
