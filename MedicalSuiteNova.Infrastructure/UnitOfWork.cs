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
            TreatmentPlanTemplates = new TreatmentPlanTemplateRepository(_context, mapper);
            TreatmentsPlanTemplateItems = new TreatmentPlanTemplateItemRepository(_context, mapper);
            SessionPlanMaster = new SessionPlanMasterRepository(_context, mapper);
            SessionPlanDetails = new SessionPlanDetailRepository(_context, mapper);
            ClinicalSessions = new ClinicalSessionRepository(_context, mapper);
            ExchangeRates = new ExchangeRateRepository(_context, mapper);
            Currencies = new CurrencyRepository(_context, mapper);
            PaymentTerms = new PaymentTermRepository(_context, mapper);
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
        public ITreatmentPlanTemplateRepository TreatmentPlanTemplates { get; private set; }
        public ITreatmentPlanTemplateItemRepository TreatmentsPlanTemplateItems { get; private set; }
        public IClinicalSessionRepository ClinicalSessions { get; private set; }
        public ISessionPlanMasterRepository SessionPlanMaster { get; private set; }
        public ISessionPlanDetailRepository SessionPlanDetails { get; private set; }
        public IExchangeRateRepository ExchangeRates { get; private set; }
        public ICurrencyRepository Currencies { get; private set; }
        public IPaymentTermRepository PaymentTerms { get; private set; }

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
