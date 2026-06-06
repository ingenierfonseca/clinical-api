using MedicalSuiteNova.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace MedicalSuiteNova.Api.Extensions.Authorization
{
    public static class ConsultationTypePolicies
    {
        public static AuthorizationBuilder AddConsultationTypePolicies(
            this AuthorizationBuilder builder)
        {
            builder
                .AddPermissionPolicy(
                    AppPolicies.CanViewConsultationTypes,
                    AppPermissions.ConsultationTypesView)
                .AddPermissionPolicy(
                    AppPolicies.CanCreateConsultationTypes,
                    AppPermissions.ConsultationTypesCreate)
                .AddPermissionPolicy(
                    AppPolicies.CanEditConsultationTypes,
                    AppPermissions.ConsultationTypesEdit);

            return builder;
        }
    }
}
