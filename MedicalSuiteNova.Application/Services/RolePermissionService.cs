
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.RolePermission;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RolePermissionService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResponse<RolePermissionDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var result = await _uow.RolePermissions.GetAllAsync(pageNumber, pageSize);
            var data = _mapper.Map<List<RolePermissionDto>>(result.Data);
            return new PagedResponse<RolePermissionDto>(data, result.CurrentPage, result.PageSize, result.TotalItems);
        }

        public async Task<RolePermissionDto?> FindAsync(int roleId, int permissionId)
        {
            var entity = await _uow.RolePermissions.FindAsync(roleId, permissionId);
            return entity == null ? null : _mapper.Map<RolePermissionDto>(entity);
        }

        public async Task<RolePermissionDto> AddAsync(CreateRolePermissionDto dto)
        {
            var exists = await _uow.RolePermissions.ExistsAsync(dto.RoleId, dto.PermissionId);
            if (exists)
                throw new ArgumentException("La asignación ya existe");

            var role = await _uow.Roles.FindAsync(dto.RoleId)
                ?? throw new KeyNotFoundException($"Rol con ID {dto.RoleId} no encontrado");
            var permission = await _uow.Permissions.FindAsync(dto.PermissionId)
                ?? throw new KeyNotFoundException($"Permiso con ID {dto.PermissionId} no encontrado");

            var entity = new RolePermission
            {
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId,
                Role = role,
                Permission = permission
            };

            await _uow.RolePermissions.AddAsync(entity);
            await _uow.CompleteAsync();

            return _mapper.Map<RolePermissionDto>(entity);
        }

        public async Task DeleteAsync(int roleId, int permissionId)
        {
            var entity = await _uow.RolePermissions.FindAsync(roleId, permissionId)
                ?? throw new KeyNotFoundException("Asignación no encontrada");

            await _uow.RolePermissions.DeleteAsync(entity);
            await _uow.CompleteAsync();
        }
    }
}
