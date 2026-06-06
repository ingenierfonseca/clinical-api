using MedicalSuiteNova.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace MedicalSuiteNova.Api.Extensions.Authorization
{
    public static class ServicePolicies
    {
        public static AuthorizationBuilder AddServicePolicies(
            this AuthorizationBuilder builder)
        {
            builder
                .AddPermissionPolicy(
                    AppPolicies.CanViewServices,
                    AppPermissions.ServicesView)
                .AddPermissionPolicy(
                    AppPolicies.CanCreateServices,
                    AppPermissions.ServicesCreate)
                .AddPermissionPolicy(
                    AppPolicies.CanEditServices,
                    AppPermissions.ServicesEdit);

            return builder;
        }
    }
}
