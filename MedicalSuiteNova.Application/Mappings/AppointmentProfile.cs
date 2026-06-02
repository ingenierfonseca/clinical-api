
using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<AppointmentDto, Appointment>();
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<AppointmentInfoDto, Appointment>();
            CreateMap<Appointment, AppointmentInfoDto>();
            CreateMap<AppointmentTypeDto, AppointmentType>();
            CreateMap<AppointmentType, AppointmentTypeDto>();
            CreateMap<ClinicalVisits, ClinicalVisitsDto>();
            CreateMap<UpdateAppointmentTypeDto, AppointmentType>();
            CreateMap<AppointmentType, UpdateAppointmentTypeDto>();
        }
    }
}
