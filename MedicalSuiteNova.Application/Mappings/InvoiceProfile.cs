
using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<InvoiceDto, Invoice>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<Invoice, InvoiceItemInfoDto>()
                .ForMember(dest => dest.CustomerName,
                       opt => opt.MapFrom(src => src.Patient!.FirstName.Trim() + " " + src.Patient.LastName.Trim()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<Invoice, InvoiceInfoDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency!.Symbol))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.InvoiceStatus!.Name))
                .ForMember(dest => dest.PaymentTerm, opt => opt.MapFrom(src => src.PaymentTerm!.Name))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.PendingBalance, opt => opt.MapFrom(src => src.Total - src.Payments.Sum(p => p.Amount)));

            CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
        }
    }
}
