using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using MedicalSuiteNova.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace MedicalSuiteNova.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            Payments = new PaymentRepository(_context, mapper);
            PaymentTypes = new PaymentTypeRepository(_context, mapper);
            Invoices = new InvoiceRepository(_context, mapper);
            Ledger = new CustomerAccountLedgerRepository(_context, mapper);
            InvoicesDetail = new InvoiceDetailRepository(_context, mapper);
            Customers = new CustomerRepository(_context, mapper);
            Appointments = new AppointmentRepository(_context, mapper);
            AppointmentTypes = new AppointmentTypeRepository(_context, mapper);
            ClinicalVisits = new ClinicalVisitsRepository(_context, mapper);
            Doctors = new DoctorRepository(_context, mapper);
            Treatments = new TreatmentRepository(_context, mapper);
            TreatmentsCategory = new TreatmentCategoryRepository(_context, mapper);
            TreatmentPlanTemplates = new TreatmentPlanTemplateRepository(_context, mapper);
            TreatmentsPlanTemplateItems = new TreatmentPlanTemplateItemRepository(_context, mapper);
            SessionPlanMaster = new SessionPlanMasterRepository(_context, mapper);
            SessionPlanDetails = new SessionPlanDetailRepository(_context, mapper);
            ClinicalSessions = new ClinicalSessionRepository(_context, mapper);
            ExchangeRates = new ExchangeRateRepository(_context, mapper);
            Currencies = new CurrencyRepository(_context, mapper);
            PaymentTerms = new PaymentTermRepository(_context, mapper);
            Users = new UserRepository(_context, mapper);
            Roles = new RoleRepository(_context, mapper);
            Permissions = new PermissionRepository(_context, mapper);
            Staff = new StaffRepository(_context, mapper);
            StaffTypes = new StaffTypeRepository(_context, mapper);
            ResourceTypes = new ResourceTypeRepository(_context, mapper);
            Resources = new ResourceRepository(_context, mapper);
            AppointmentStatuses = new AppointmentStatusRepository(_context, mapper);
            RolePermissions = new RolePermissionRepository(_context, mapper);
        }

        public IPaymentRepository Payments { get; private set; }
        public IPaymentTypeRepository PaymentTypes { get; private set; }
        public IInvoiceRepository Invoices { get; private set; }
        public ICustomerAccountLedgerRepository Ledger { get; private set; }
        public IInvoiceDetailRepository InvoicesDetail { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IAppointmentRepository Appointments { get; private set; }
        public IAppointmentTypeRepository AppointmentTypes { get; private set; }
        public IClinicalVisitsRepository ClinicalVisits { get; private set; }
        public IDoctorRepository Doctors { get; private set; }
        public ITreatmentRepository Treatments { get; private set; }
        public ITreatmentCategoryRepository TreatmentsCategory { get; private set; }
        public ITreatmentPlanTemplateRepository TreatmentPlanTemplates { get; private set; }
        public ITreatmentPlanTemplateItemRepository TreatmentsPlanTemplateItems { get; private set; }
        public IClinicalSessionRepository ClinicalSessions { get; private set; }
        public ISessionPlanMasterRepository SessionPlanMaster { get; private set; }
        public ISessionPlanDetailRepository SessionPlanDetails { get; private set; }
        public IExchangeRateRepository ExchangeRates { get; private set; }
        public ICurrencyRepository Currencies { get; private set; }
        public IPaymentTermRepository PaymentTerms { get; private set; }
        public IUserRepository Users { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IPermissionRepository Permissions { get; private set; }
        public IStaffRepository Staff { get; private set; }
        public IStaffTypeRepository StaffTypes { get; private set; }
        public IResourceTypeRepository ResourceTypes { get; private set; }
        public IResourceRepository Resources { get; private set; }
        public IAppointmentStatusRepository AppointmentStatuses { get; private set; }
        public IRolePermissionRepository RolePermissions { get; private set; }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            await _transaction!.CommitAsync();
            _transaction.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction!.RollbackAsync();
            _transaction.Dispose();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
