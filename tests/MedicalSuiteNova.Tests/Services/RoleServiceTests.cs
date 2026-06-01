using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Application.Services;
using MedicalSuiteNova.Domain.Entities;
using Moq;
using AutoMapper;

namespace MedicalSuiteNova.Tests.Services;

public class RoleServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RoleService _roleService;

    public RoleServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _roleService = new RoleService(_mockUow.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task AssignRolesToUser_Valid_ReplacesRoles()
    {
        var user = new User { Id = 1, Username = "test", Email = "test@test.com", PasswordHash = "hash" };
        var role1 = new Role { Id = 1, Name = "Admin", IsActive = true };
        var role2 = new Role { Id = 2, Name = "Doctor", IsActive = true };

        _mockUow.Setup(x => x.Users.FindAsync(1)).ReturnsAsync(user);
        _mockUow.Setup(x => x.Roles.FindAsync(1)).ReturnsAsync(role1);
        _mockUow.Setup(x => x.Roles.FindAsync(2)).ReturnsAsync(role2);
        _mockUow.Setup(x => x.Users.SetRolesAsync(1, new List<int> { 1, 2 }))
            .Returns(Task.CompletedTask);
        _mockUow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

        await _roleService.AssignRolesToUserAsync(1, new List<int> { 1, 2 });

        _mockUow.Verify(x => x.Users.SetRolesAsync(1, It.Is<List<int>>(ids => ids.SequenceEqual(new[] { 1, 2 }))), Times.Once);
        _mockUow.Verify(x => x.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task AssignRolesToUser_UserNotFound_ThrowsKeyNotFoundException()
    {
        _mockUow.Setup(x => x.Users.FindAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        var act = () => _roleService.AssignRolesToUserAsync(99, new List<int> { 1 });

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Usuario no encontrado");
    }

    [Fact]
    public async Task AssignRolesToUser_RoleNotFound_ThrowsKeyNotFoundException()
    {
        var user = new User { Id = 1, Username = "test", Email = "test@test.com", PasswordHash = "hash" };
        _mockUow.Setup(x => x.Users.FindAsync(1)).ReturnsAsync(user);
        _mockUow.Setup(x => x.Roles.FindAsync(It.IsAny<int>()))
            .ReturnsAsync((Role?)null);

        var act = () => _roleService.AssignRolesToUserAsync(1, new List<int> { 99 });

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Rol con ID 99 no encontrado");
    }

    [Fact]
    public async Task AssignRolesToUser_InactiveRole_ThrowsArgumentException()
    {
        var user = new User { Id = 1, Username = "test", Email = "test@test.com", PasswordHash = "hash" };
        var inactiveRole = new Role { Id = 3, Name = "Inactivo", IsActive = false };

        _mockUow.Setup(x => x.Users.FindAsync(1)).ReturnsAsync(user);
        _mockUow.Setup(x => x.Roles.FindAsync(3)).ReturnsAsync(inactiveRole);

        var act = () => _roleService.AssignRolesToUserAsync(1, new List<int> { 3 });

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("El rol 'Inactivo' está inactivo");
    }

    [Fact]
    public async Task AssignPermissionsToRole_Valid_ReplacesPermissions()
    {
        var role = new Role { Id = 1, Name = "Admin", IsActive = true };
        var perm1 = new Permission { Id = 1, Name = "users.manage", Module = "Usuarios" };
        var perm2 = new Permission { Id = 2, Name = "reports.read", Module = "Reportes" };

        _mockUow.Setup(x => x.Roles.FindAsync(1)).ReturnsAsync(role);
        _mockUow.Setup(x => x.Permissions.FindAsync(1)).ReturnsAsync(perm1);
        _mockUow.Setup(x => x.Permissions.FindAsync(2)).ReturnsAsync(perm2);
        _mockUow.Setup(x => x.Roles.SetPermissionsAsync(1, new List<int> { 1, 2 }))
            .Returns(Task.CompletedTask);
        _mockUow.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

        await _roleService.AssignPermissionsToRoleAsync(1, new List<int> { 1, 2 });

        _mockUow.Verify(x => x.Roles.SetPermissionsAsync(1, It.Is<List<int>>(ids => ids.SequenceEqual(new[] { 1, 2 }))), Times.Once);
        _mockUow.Verify(x => x.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task AssignPermissionsToRole_RoleNotFound_ThrowsKeyNotFoundException()
    {
        _mockUow.Setup(x => x.Roles.FindAsync(It.IsAny<int>()))
            .ReturnsAsync((Role?)null);

        var act = () => _roleService.AssignPermissionsToRoleAsync(99, new List<int> { 1 });

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Rol no encontrado");
    }

    [Fact]
    public async Task AssignPermissionsToRole_PermissionNotFound_ThrowsKeyNotFoundException()
    {
        var role = new Role { Id = 1, Name = "Admin", IsActive = true };
        _mockUow.Setup(x => x.Roles.FindAsync(1)).ReturnsAsync(role);
        _mockUow.Setup(x => x.Permissions.FindAsync(It.IsAny<int>()))
            .ReturnsAsync((Permission?)null);

        var act = () => _roleService.AssignPermissionsToRoleAsync(1, new List<int> { 99 });

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Permiso con ID 99 no encontrado");
    }

    [Fact]
    public async Task GetRolePermissionIds_ReturnsIds()
    {
        var expectedIds = new List<int> { 1, 2, 3 };
        _mockUow.Setup(x => x.Roles.GetPermissionIdsAsync(1))
            .ReturnsAsync(expectedIds);

        var result = await _roleService.GetRolePermissionIdsAsync(1);

        result.Should().BeEquivalentTo(expectedIds);
    }
}
