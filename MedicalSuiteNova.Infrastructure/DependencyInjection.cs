using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using MedicalSuiteNova.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalSuiteNova.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                )
            );

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentTypeRepository, AppointmentTypeRepository>();
            services.AddScoped<IClinicalVisitsRepository, ClinicalVisitsRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddScoped<ICustomerAccountLedgerRepository, CustomerAccountLedgerRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<ITreatmentRepository, TreatmentRepository>();
            services.AddScoped<ITreatmentPlanTemplateRepository, TreatmentPlanTemplateRepository>();
            services.AddScoped<ITreatmentPlanTemplateItemRepository, TreatmentPlanTemplateItemRepository>();
            services.AddScoped<IClinicalSessionRepository, ClinicalSessionRepository>();
            services.AddScoped<ISessionPlanMasterRepository, SessionPlanMasterRepository>();
            services.AddScoped<ISessionPlanDetailRepository, SessionPlanDetailRepository>();
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IPaymentTermRepository, PaymentTermRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
