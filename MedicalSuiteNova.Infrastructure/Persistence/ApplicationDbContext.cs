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
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceItem> InvoiceDetails => Set<InvoiceItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<InvoiceStatus> InvoiceStatus => Set<InvoiceStatus>();
        public DbSet<PaymentTerm> PaymentTerms => Set<PaymentTerm>();
        public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<AppointmentType>().ToTable("AppointmentType");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItem");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<InvoiceStatus>().ToTable("InvoiceStatus");
            modelBuilder.Entity<PaymentTerm>().ToTable("PaymentTerm");
            modelBuilder.Entity<PaymentType>().ToTable("PaymentType");
            base.OnModelCreating(modelBuilder);
        }
    }
}
