using MedicalSuiteNova.Api.Extensions.Authorization;

namespace MedicalSuiteNova.Api.Middlewares
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPaymentPolicies()
                .AddTreatmentPolicies()
                .AddServicePolicies()
                .AddSpecialtyPolicies()
                .AddConsultationTypePolicies();

            return services;
        }
    }
}
