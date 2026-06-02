
using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class StaffProfile : Profile
    {
        public StaffProfile()
        {
            CreateMap<StaffType, StaffTypeDto>().ReverseMap();
            CreateMap<Staff, StaffDto>().ReverseMap();
        }
    }
}
