using MedicalSuiteNova.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace MedicalSuiteNova.Api.Extensions.Authorization
{
    public static class PaymentPolicies
    {
        public static AuthorizationBuilder AddPaymentPolicies(
            this AuthorizationBuilder builder)
        {
            builder
                .AddPermissionPolicy(
                    AppPolicies.CanViewPayments,
                    AppPermissions.PaymentsView)
                .AddPermissionPolicy(
                    AppPolicies.CanCreatePayments,
                    AppPermissions.PaymentsCreate)
                .AddPermissionPolicy(
                    AppPolicies.CanEditPayments,
                    AppPermissions.PaymentsEdit);

            return builder;
        }
    }
}
