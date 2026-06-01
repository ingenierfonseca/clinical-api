using MedicalSuiteNova.Api.Services;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;

namespace MedicalSuiteNova.Tests.Services;

public class TokenServiceTests
{
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        _mockConfig = new Mock<IConfiguration>();

        _mockConfig.Setup(x => x["Jwt:Key"]).Returns("UnaClaveSuperSecretaDeAlMenos32Caracteres!");
        _mockConfig.Setup(x => x["Jwt:Issuer"]).Returns("MedicalSuiteNova.API");
        _mockConfig.Setup(x => x["Jwt:Audience"]).Returns("MedicalSuiteNova.App");

        _tokenService = new TokenService(_mockConfig.Object);
    }

    private static User CreateUser()
    {
        return new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@test.com",
            PasswordHash = "hash",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UserRoles = new List<UserRole>
            {
                new()
                {
                    UserId = 1,
                    RoleId = 1,
                    Role = new Role
                    {
                        Id = 1,
                        Name = "Admin",
                        IsActive = true,
                        RolePermissions = new List<RolePermission>
                        {
                            new()
                            {
                                RoleId = 1,
                                PermissionId = 1,
                                Permission = new Permission
                                {
                                    Id = 1,
                                    Name = "users.manage",
                                    Module = "Usuarios"
                                }
                            },
                            new()
                            {
                                RoleId = 1,
                                PermissionId = 2,
                                Permission = new Permission
                                {
                                    Id = 2,
                                    Name = "reports.read",
                                    Module = "Reportes"
                                }
                            }
                        }
                    }
                },
                new()
                {
                    UserId = 1,
                    RoleId = 2,
                    Role = new Role
                    {
                        Id = 2,
                        Name = "Doctor",
                        IsActive = true,
                        RolePermissions = new List<RolePermission>
                        {
                            new()
                            {
                                RoleId = 2,
                                PermissionId = 1,
                                Permission = new Permission
                                {
                                    Id = 1,
                                    Name = "users.manage",
                                    Module = "Usuarios"
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    [Fact]
    public void CreateToken_ContainsSubClaim()
    {
        var user = CreateUser();
        var token = _tokenService.CreateToken(user);

        token.Should().NotBeNullOrEmpty();

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Subject.Should().Be(user.Username);
    }

    [Fact]
    public void CreateToken_ContainsRoleClaims()
    {
        var user = CreateUser();
        var token = _tokenService.CreateToken(user);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var roleClaims = jwtToken.Claims
            .Where(c => c.Type == "role")
            .Select(c => c.Value)
            .ToList();

        roleClaims.Should().Contain("Admin");
        roleClaims.Should().Contain("Doctor");
    }

    [Fact]
    public void CreateToken_ContainsPermissionClaims()
    {
        var user = CreateUser();
        var token = _tokenService.CreateToken(user);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var permissionClaims = jwtToken.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .Distinct()
            .ToList();

        permissionClaims.Should().Contain("users.manage");
        permissionClaims.Should().Contain("reports.read");
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsUniqueTokens()
    {
        var token1 = _tokenService.GenerateRefreshToken();
        var token2 = _tokenService.GenerateRefreshToken();

        token1.Should().NotBeNullOrEmpty();
        token2.Should().NotBeNullOrEmpty();
        token1.Should().NotBe(token2);
    }

    [Fact]
    public void CreateToken_NoUserRoles_ContainsOnlySubClaim()
    {
        var user = new User
        {
            Id = 2,
            Username = "noroles",
            Email = "no@test.com",
            PasswordHash = "hash",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UserRoles = new List<UserRole>()
        };

        var token = _tokenService.CreateToken(user);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Subject.Should().Be("noroles");
        var roleClaims = jwtToken.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        roleClaims.Should().BeEmpty();
    }
}
