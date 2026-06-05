using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Appointment;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<AppointmentDto, Appointment>()
                //.ForMember(dest => dest.EndTime, opt => opt.Ignore())
                //.ForMember(dest => dest.StatusId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<AppointmentInfoDto, Appointment>();
            CreateMap<Appointment, AppointmentInfoDto>();
            CreateMap<AppointmentTypeDto, AppointmentType>();
            CreateMap<AppointmentType, AppointmentTypeDto>();
            CreateMap<ClinicalVisits, ClinicalVisitsDto>();
            CreateMap<UpdateAppointmentTypeDto, AppointmentType>();
            CreateMap<AppointmentType, UpdateAppointmentTypeDto>();
            CreateMap<AppointmentStatus, AppointmentStatusDto>().ReverseMap();
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<AppointmentDto, CreateAppointmentDto>();
        }
    }
}
