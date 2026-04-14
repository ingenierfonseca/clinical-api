using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<InvoiceDto, Invoice>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<Invoice, InvoiceItemInfoDto>()
                .ForMember(dest => dest.CustomerName,
                       opt => opt.MapFrom(src => src.Patient!.FirstName.Trim() + " " + src.Patient.LastName.Trim()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<Invoice, InvoiceInfoDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.Symbol))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.InvoiceStatus.Name))
                .ForMember(dest => dest.PaymentTerm, opt => opt.MapFrom(src => src.PaymentTerm.Name))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src =>
                    /*(src.StatusId == (int)InvoiceStatusEnum.Pendiente
                    || src.StatusId == (int)InvoiceStatusEnum.PagoParcial
                    || src.StatusId == (int)InvoiceStatusEnum.Vencida)
                    ? src.Total - src.Payments.Sum(p => p.Amount)
                    : */src.Total
                ));

            CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
            CreateMap<AppointmentDto, Appointment>();
            CreateMap<Appointment, AppointmentDto>();
        }
    }
}
