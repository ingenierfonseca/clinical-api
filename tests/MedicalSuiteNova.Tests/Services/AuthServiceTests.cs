using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Application.Services;
using MedicalSuiteNova.Domain.Dto.Auth;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using Moq;

namespace MedicalSuiteNova.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<ITokenService> _mockToken;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockToken = new Mock<ITokenService>();
        _authService = new AuthService(_mockUow.Object, _mockToken.Object);
    }

    private static User CreateUser(string username = "testuser", bool isActive = true)
    {
        return new User
        {
            Id = 1,
            Username = username,
            Email = "test@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            IsActive = isActive,
            RefreshToken = null,
            RefreshTokenExpiry = null,
            CreatedAt = DateTime.UtcNow,
            UserRoles = new List<UserRole>
            {
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
                                    Name = "appointments.read",
                                    Module = "Citas"
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsAuthResponse()
    {
        var user = CreateUser();
        _mockUow.Setup(x => x.Users.GetByUsernameAsync(user.Username))
            .ReturnsAsync(user);
        _mockToken.Setup(x => x.CreateToken(user)).Returns("jwt-token");
        _mockToken.Setup(x => x.GenerateRefreshToken()).Returns("refresh-token");

        var result = await _authService.LoginAsync(new LoginRequestDto
        {
            Username = user.Username,
            Password = "password123"
        });

        result.Should().NotBeNull();
        result.Token.Should().Be("jwt-token");
        result.RefreshToken.Should().Be("refresh-token");
        result.Username.Should().Be(user.Username);
        result.Roles.Should().Contain("Doctor");
        result.Permissions.Should().Contain("appointments.read");
    }

    [Fact]
    public async Task Login_InvalidPassword_ThrowsUnauthorizedAccessException()
    {
        var user = CreateUser();
        _mockUow.Setup(x => x.Users.GetByUsernameAsync(user.Username))
            .ReturnsAsync(user);

        var act = () => _authService.LoginAsync(new LoginRequestDto
        {
            Username = user.Username,
            Password = "wrongpassword"
        });

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Usuario o contraseña incorrectos");
    }

    [Fact]
    public async Task Login_UserNotFound_ThrowsUnauthorizedAccessException()
    {
        _mockUow.Setup(x => x.Users.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var act = () => _authService.LoginAsync(new LoginRequestDto
        {
            Username = "nonexistent",
            Password = "password123"
        });

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Usuario o contraseña incorrectos");
    }

    [Fact]
    public async Task Login_InactiveUser_ThrowsUnauthorizedAccessException()
    {
        var user = CreateUser(isActive: false);
        _mockUow.Setup(x => x.Users.GetByUsernameAsync(user.Username))
            .ReturnsAsync(user);

        var act = () => _authService.LoginAsync(new LoginRequestDto
        {
            Username = user.Username,
            Password = "password123"
        });

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("La cuenta está desactivada");
    }

    [Fact]
    public async Task Register_NewUser_CreatesWithDoctorRole()
    {
        var defaultRole = new Role { Id = 2, Name = "Doctor", IsActive = true };
        _mockUow.Setup(x => x.Roles.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(),
                It.IsAny<System.Linq.Expressions.Expression<Func<Role, object>>[]>()))
            .ReturnsAsync(defaultRole);
        _mockUow.Setup(x => x.Users.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        _mockUow.Setup(x => x.Users.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);
        _mockUow.Setup(x => x.Users.SetRolesAsync(It.IsAny<int>(), It.IsAny<List<int>>()))
            .Returns(Task.CompletedTask);
        _mockUow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);
        _mockToken.Setup(x => x.CreateToken(It.IsAny<User>())).Returns("jwt-token");
        _mockToken.Setup(x => x.GenerateRefreshToken()).Returns("refresh-token");

        var result = await _authService.RegisterAsync(new RegisterRequestDto
        {
            Username = "newuser",
            Email = "new@test.com",
            Password = "password123"
        });

        result.Should().NotBeNull();
        result.Username.Should().Be("newuser");
    }

    [Fact]
    public async Task Register_DuplicateUsername_ThrowsArgumentException()
    {
        var existing = CreateUser();
        _mockUow.Setup(x => x.Users.GetByUsernameAsync(existing.Username))
            .ReturnsAsync(existing);

        var act = () => _authService.RegisterAsync(new RegisterRequestDto
        {
            Username = existing.Username,
            Email = "another@test.com",
            Password = "password123"
        });

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("El nombre de usuario ya existe");
    }

    [Fact]
    public async Task RefreshToken_Valid_RotatesToken()
    {
        var user = CreateUser();
        user.RefreshToken = "valid-refresh-token";
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);

        _mockUow.Setup(x => x.Users.GetByRefreshTokenAsync("valid-refresh-token"))
            .ReturnsAsync(user);
        _mockToken.Setup(x => x.CreateToken(user)).Returns("new-jwt-token");
        _mockToken.Setup(x => x.GenerateRefreshToken()).Returns("new-refresh-token");

        var result = await _authService.RefreshTokenAsync(new RefreshTokenRequestDto
        {
            RefreshToken = "valid-refresh-token"
        });

        result.Should().NotBeNull();
        result.Token.Should().Be("new-jwt-token");
        result.RefreshToken.Should().Be("new-refresh-token");
    }

    [Fact]
    public async Task RefreshToken_Expired_ThrowsUnauthorizedAccessException()
    {
        var user = CreateUser();
        user.RefreshToken = "expired-refresh-token";
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(-1);

        _mockUow.Setup(x => x.Users.GetByRefreshTokenAsync("expired-refresh-token"))
            .ReturnsAsync(user);

        var act = () => _authService.RefreshTokenAsync(new RefreshTokenRequestDto
        {
            RefreshToken = "expired-refresh-token"
        });

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Refresh token inválido o expirado");
    }

    [Fact]
    public async Task LinkDoctor_Valid_UpdatesUser()
    {
        var user = CreateUser();
            var doctor = new Doctor { Id = 5, FirstName = "Juan", LastName = "Perez", Specialist = "Odontología", Phone = "555-0000" };

        _mockUow.Setup(x => x.Users.FindAsync(1)).ReturnsAsync(user);
        _mockUow.Setup(x => x.Doctors.FindAsync(5)).ReturnsAsync(doctor);

        var result = await _authService.LinkDoctorAsync(1, 5);

        result.Should().NotBeNull();
        result.DoctorId.Should().Be(5);
        result.Message.Should().Contain("Juan");
        _mockUow.Verify(x => x.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task LinkDoctor_UserNotFound_ThrowsKeyNotFoundException()
    {
        _mockUow.Setup(x => x.Users.FindAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        var act = () => _authService.LinkDoctorAsync(99, 5);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Usuario no encontrado");
    }

    [Fact]
    public async Task LinkCustomer_Valid_UpdatesUser()
    {
        var user = CreateUser();
            var customer = new Customer { Id = 3, DNI = "001-000000-0", FirstName = "Maria", LastName = "Lopez" };

        _mockUow.Setup(x => x.Users.FindAsync(1)).ReturnsAsync(user);
        _mockUow.Setup(x => x.Customers.FindAsync(3)).ReturnsAsync(customer);

        var result = await _authService.LinkCustomerAsync(1, 3);

        result.Should().NotBeNull();
        result.CustomerId.Should().Be(3);
        result.Message.Should().Contain("Maria");
    }

    [Fact]
    public async Task GetCurrentUser_ReturnsUserInfoWithRolesAndPermissions()
    {
        var user = CreateUser();
        _mockUow.Setup(x => x.Users.GetByUsernameAsync(user.Username))
            .ReturnsAsync(user);

        var result = await _authService.GetCurrentUserAsync(user.Username);

        result.Should().NotBeNull();
        result.Username.Should().Be(user.Username);
        result.Roles.Should().Contain("Doctor");
        result.Permissions.Should().Contain("appointments.read");
    }

    [Fact]
    public async Task GetCurrentUser_UserNotFound_ThrowsKeyNotFoundException()
    {
        _mockUow.Setup(x => x.Users.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var act = () => _authService.GetCurrentUserAsync("nonexistent");

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Usuario no encontrado");
    }
}
