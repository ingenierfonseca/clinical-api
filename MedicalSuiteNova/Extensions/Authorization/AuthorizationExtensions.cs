using MedicalSuiteNova.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace MedicalSuiteNova.Api.Extensions.Authorization
{
    public static class AuthorizationExtensions
    {
        public static AuthorizationBuilder AddPermissionPolicy(
        this AuthorizationBuilder builder,
        string policy,
        string permission)
        {
            builder.AddPolicy(policy, p =>
                p.RequireAssertion(ctx =>
                    ctx.User.IsInRole(AppRole.SuperAdmin) ||
                    ctx.User.HasClaim("permission", permission)));

            return builder;
        }
    }
}
