using MedicalSuiteNova.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace MedicalSuiteNova.Api.Extensions.Authorization
{
    public static class SpecialtyPolicies
    {
        public static AuthorizationBuilder AddSpecialtyPolicies(
            this AuthorizationBuilder builder)
        {
            builder
                .AddPermissionPolicy(
                    AppPolicies.CanViewSpecialties,
                    AppPermissions.SpecialtiesView)
                .AddPermissionPolicy(
                    AppPolicies.CanCreateSpecialties,
                    AppPermissions.SpecialtiesCreate)
                .AddPermissionPolicy(
                    AppPolicies.CanEditSpecialties,
                    AppPermissions.SpecialtiesEdit);

            return builder;
        }
    }
}
