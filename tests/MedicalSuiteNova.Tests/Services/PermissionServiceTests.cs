using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Application.Services;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using Moq;
using AutoMapper;

namespace MedicalSuiteNova.Tests.Services;

public class PermissionServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPermissionRepository> _mockRepo;
    private readonly PermissionService _permissionService;

    public PermissionServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockRepo = new Mock<IPermissionRepository>();

        _mockUow.Setup(x => x.Permissions).Returns(_mockRepo.Object);

        _permissionService = new PermissionService(_mockUow.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllPermissions_ReturnsPagedResponse()
    {
        var permissions = new List<Permission>
        {
            new() { Id = 1, Name = "users.manage", Module = "Usuarios" },
            new() { Id = 2, Name = "reports.read", Module = "Reportes" }
        };
        var expectedDto = new List<object> { new { Id = 1, Name = "users.manage" } };

        _mockUow.Setup(x => x.Permissions.GetAllAsync<object>(1, 10, null, null))
            .ReturnsAsync(new PagedResponse<object>(expectedDto, 1, 10, 2));

        var result = await _permissionService.GetAllAsync<object>(1, 10);

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(1);
        result.TotalItems.Should().Be(2);
    }

    [Fact]
    public async Task FindPermission_ReturnsEntity()
    {
        var permission = new Permission { Id = 1, Name = "users.manage", Module = "Usuarios" };
        _mockUow.Setup(x => x.Permissions.FindAsync(1)).ReturnsAsync(permission);

        var result = await _permissionService.FindAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("users.manage");
    }
}
