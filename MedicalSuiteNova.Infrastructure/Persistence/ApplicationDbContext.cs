using Microsoft.EntityFrameworkCore;
using MedicalSuiteNova.Domain.Entities;


namespace MedicalSuiteNova.Infrastructure.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Customer> Patients => Set<Customer>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AppointmentType> AppointmentTypes => Set<AppointmentType>();
        public DbSet<ClinicalVisits> ClinicalVisits => Set<ClinicalVisits>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceItem> InvoiceDetails => Set<InvoiceItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<CustomerAccountLedger> CustomerAccounts => Set<CustomerAccountLedger>();
        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<InvoiceStatus> InvoiceStatus => Set<InvoiceStatus>();
        public DbSet<PaymentTerm> PaymentTerms => Set<PaymentTerm>();
        public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
        public DbSet<Treatment> Treatments => Set<Treatment>();
        public DbSet<TreatmentPlanTemplate> TreatmentPlanTemplates => Set<TreatmentPlanTemplate>();
        public DbSet<TreatmentPlanTemplateItem> TreatmentPlanTemplateItems => Set<TreatmentPlanTemplateItem>();
        public DbSet<ClinicalSession> ClinicalSessions => Set<ClinicalSession>();
        public DbSet<SessionPlanMaster> SessionPlanMasters => Set<SessionPlanMaster>();
        public DbSet<SessionPlanDetail> SessionPlanDetails => Set<SessionPlanDetail>();
        public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<AppointmentType>().ToTable("AppointmentType");
            modelBuilder.Entity<ClinicalVisits>().ToTable("ClinicalVisits");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItem");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<CustomerAccountLedger>().ToTable("CustomerAccountLedger");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<InvoiceStatus>().ToTable("InvoiceStatus");
            modelBuilder.Entity<PaymentTerm>().ToTable("PaymentTerm");
            modelBuilder.Entity<PaymentType>().ToTable("PaymentType");
            modelBuilder.Entity<Treatment>().ToTable("Treatment");
            modelBuilder.Entity<TreatmentPlanTemplate>().ToTable("TreatmentPlanTemplate");
            modelBuilder.Entity<TreatmentPlanTemplateItem>().ToTable("TreatmentPlanTemplateItem");
            modelBuilder.Entity<ClinicalSession>().ToTable("ClinicalSession");
            modelBuilder.Entity<SessionPlanMaster>().ToTable("SessionPlanMaster");
            modelBuilder.Entity<SessionPlanDetail>().ToTable("SessionPlanDetail");
            modelBuilder.Entity<ExchangeRate>().ToTable("ExchangeRates");
            base.OnModelCreating(modelBuilder);
        }
    }
}
