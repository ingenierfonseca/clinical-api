
using AutoMapper;
using MedicalSuiteNova.Domain.Dto.Permission;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionDto, Permission>();
            CreateMap<Permission, PermissionDto>();
            CreateMap<CreatePermissionDto, Permission>();
            CreateMap<Permission, CreatePermissionDto>();
            CreateMap<UpdatePermissionDto, Permission>();
            CreateMap<Permission, UpdatePermissionDto>();
        }
    }
}
