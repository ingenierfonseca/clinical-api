
using AutoMapper;
using MedicalSuiteNova.Domain.Dto.Doctor;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctor, DoctorDto>();
            CreateMap<Doctor, UpdateDoctorDto>();
            CreateMap<Doctor, DoctorInfoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.FirstName : string.Empty))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.LastName : string.Empty));
        }
    }
}
