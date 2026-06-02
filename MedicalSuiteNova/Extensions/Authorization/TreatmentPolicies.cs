using MedicalSuiteNova.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace MedicalSuiteNova.Api.Extensions.Authorization
{
    public static class TreatmentPolicies
    {
        public static AuthorizationBuilder AddTreatmentPolicies(
            this AuthorizationBuilder builder)
        {
            builder
                .AddPermissionPolicy(
                    AppPolicies.CanViewTreatments,
                    AppPermissions.TreatmentsView)
                .AddPermissionPolicy(
                    AppPolicies.CanCreateTreatments,
                    AppPermissions.TreatmentsCreate)
                .AddPermissionPolicy(
                    AppPolicies.CanEditTreatments,
                    AppPermissions.TreatmentsEdit);

            return builder;
        }
    }
}
