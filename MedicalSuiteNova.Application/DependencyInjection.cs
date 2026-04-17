using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Application.Mappings;
using MedicalSuiteNova.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace MedicalSuiteNova.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(sp =>
            {
                var configExpression = new MapperConfigurationExpression();
                configExpression.AddProfile<MappingProfile>();

                var config = new MapperConfiguration(
                    configExpression,
                    NullLoggerFactory.Instance
                );

                return config.CreateMapper();
            });
            /*var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mappingConfig);
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);*/

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentTypeService, AppointmentTypeService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ITreatmentService, TreatmentService>();

            return services;
        }
    }
}
