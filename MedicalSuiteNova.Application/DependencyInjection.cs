using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;

namespace MedicalSuiteNova.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(sp =>
            {
                var config = new MapperConfiguration(
                    cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()),
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
            services.AddScoped<ITreatmentCategoryService, TreatmentCategoryService>();
            services.AddScoped<ITreatmentPlanTemplateService, TreatmentPlanTemplateService>();
            services.AddScoped<IClinicalSessionService, ClinicalSessionService>();
            services.AddScoped<ISessionPlanMasterService, SessionPlanMasterService>();
            services.AddScoped<IPaymentTermService, PaymentTermService>();
            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStaffTypeService, StaffTypeService>();
            services.AddScoped<IResourceTypeService, ResourceTypeService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IAppointmentStatusService, AppointmentStatusService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IConsultationTypeService, ConsultationTypeService>();
  
            return services;
        }
    }
}
