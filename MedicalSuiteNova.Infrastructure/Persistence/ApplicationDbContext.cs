using Microsoft.EntityFrameworkCore;
using MedicalSuiteNova.Domain.Entities;


namespace MedicalSuiteNova.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
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
        public DbSet<TreatmentCategory> TreatmentCategories => Set<TreatmentCategory>();
        public DbSet<TreatmentPlanTemplate> TreatmentPlanTemplates => Set<TreatmentPlanTemplate>();
        public DbSet<TreatmentPlanTemplateItem> TreatmentPlanTemplateItems => Set<TreatmentPlanTemplateItem>();
        public DbSet<ClinicalSession> ClinicalSessions => Set<ClinicalSession>();
        public DbSet<SessionPlanMaster> SessionPlanMasters => Set<SessionPlanMaster>();
        public DbSet<SessionPlanDetail> SessionPlanDetails => Set<SessionPlanDetail>();
        public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<StaffType> StaffTypes => Set<StaffType>();
        public DbSet<Staff> Staff => Set<Staff>();
        public DbSet<ResourceType> ResourceTypes => Set<ResourceType>();
        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<AppointmentStatus> AppointmentStatuses => Set<AppointmentStatus>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Specialty> Specialties => Set<Specialty>();
        public DbSet<ConsultationType> ConsultationTypes => Set<ConsultationType>();
  
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
            modelBuilder.Entity<TreatmentCategory>().ToTable("TreatmentCategory");
            modelBuilder.Entity<TreatmentPlanTemplate>().ToTable("TreatmentPlanTemplate");
            modelBuilder.Entity<TreatmentPlanTemplateItem>().ToTable("TreatmentPlanTemplateItem");
            modelBuilder.Entity<ClinicalSession>().ToTable("ClinicalSession");
            modelBuilder.Entity<SessionPlanMaster>().ToTable("SessionPlanMaster");
            modelBuilder.Entity<SessionPlanDetail>().ToTable("SessionPlanDetail");
            modelBuilder.Entity<ExchangeRate>().ToTable("ExchangeRates");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<UserRole>().ToTable("UserRole").HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<StaffType>().ToTable("StaffType");
            modelBuilder.Entity<Staff>().ToTable("Staff")
                .HasOne(s => s.StaffType)
                .WithMany()
                .HasForeignKey(s => s.StaffTypeId);
            modelBuilder.Entity<ResourceType>().ToTable("ResourceType");
            modelBuilder.Entity<Resource>().ToTable("Resource")
                .HasOne(r => r.ResourceType)
                .WithMany()
                .HasForeignKey(r => r.ResourceTypeId);
            modelBuilder.Entity<AppointmentStatus>().ToTable("AppointmentStatus");
            modelBuilder.Entity<RolePermission>().ToTable("RolePermission").HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<Service>().ToTable("Services");
            modelBuilder.Entity<Specialty>().ToTable("Specialties");
            modelBuilder.Entity<ConsultationType>().ToTable("ConsultationType");
            base.OnModelCreating(modelBuilder);
        }
    }
}
