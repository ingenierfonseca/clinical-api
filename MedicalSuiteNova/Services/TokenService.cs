using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MedicalSuiteNova.Api.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        private readonly IConfiguration _config = config;

        public string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Username),
                new (JwtRegisteredClaimNames.UniqueName, user.Username),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user.UserRoles != null)
            {
                var uniqueRoles = new HashSet<string>();
                var uniquePermissions = new HashSet<string>();

                foreach (var userRole in user.UserRoles)
                {
                    if (userRole.Role?.Name != null)
                        uniqueRoles.Add(userRole.Role.Name);

                    if (userRole.Role?.RolePermissions != null)
                    {
                        foreach (var rolePermission in userRole.Role.RolePermissions)
                        {
                            if (rolePermission.Permission?.Name != null)
                                uniquePermissions.Add(rolePermission.Permission.Name);
                        }
                    }
                }

                foreach (var role in uniqueRoles)
                    claims.Add(new Claim(ClaimTypes.Role, role));

                foreach (var permission in uniquePermissions)
                    claims.Add(new Claim("permission", permission));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
