
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPaymentRepository Payments { get; }
        IPaymentTypeRepository PaymentTypes { get; }
        IInvoiceRepository Invoices { get; }
        ICustomerAccountLedgerRepository Ledger {  get; }
        IInvoiceDetailRepository InvoicesDetail { get; }
        ICustomerRepository Customers { get; }
        IAppointmentRepository Appointments { get; }
        IAppointmentTypeRepository AppointmentTypes { get; }
        IClinicalVisitsRepository ClinicalVisits { get; }
        IDoctorRepository Doctors { get; }
        ITreatmentRepository Treatments { get; }
        ITreatmentCategoryRepository TreatmentsCategory { get; }
        ITreatmentPlanTemplateRepository TreatmentPlanTemplates { get; }
        ITreatmentPlanTemplateItemRepository TreatmentsPlanTemplateItems { get; }
        IClinicalSessionRepository ClinicalSessions { get; }
        ISessionPlanMasterRepository SessionPlanMaster { get; }
        ISessionPlanDetailRepository SessionPlanDetails { get; }
        IExchangeRateRepository ExchangeRates { get; }
        ICurrencyRepository Currencies { get; }
        IPaymentTermRepository PaymentTerms { get; }
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IStaffRepository Staff { get; }
        IStaffTypeRepository StaffTypes { get; }
        IResourceTypeRepository ResourceTypes { get; }
        IResourceRepository Resources { get; }
        IAppointmentStatusRepository AppointmentStatuses { get; }
        IRolePermissionRepository RolePermissions { get; }
        IServiceRepository Services { get; }
        ISpecialtyRepository Specialties { get; }
        IConsultationTypeRepository ConsultationTypes { get; }
        IClinicalNotesRepository ClinicalNotes { get; }
        IClinicalFileRepository ClinicalFiles { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
