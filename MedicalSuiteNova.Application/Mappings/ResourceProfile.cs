using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<ResourceType, ResourceTypeDto>().ReverseMap();
            CreateMap<Resource, ResourceDto>().ReverseMap();
        }
    }
}
