
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
            CreateMap<DoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();
            CreateMap<Doctor, DoctorInfoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.FirstName : string.Empty))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.LastName : string.Empty))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.Avatar : string.Empty))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.BirthDate : null))
            .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Service != null ? src.Service.Name : string.Empty))
            .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty != null ? src.Specialty.Name : string.Empty));
        }
    }
}
