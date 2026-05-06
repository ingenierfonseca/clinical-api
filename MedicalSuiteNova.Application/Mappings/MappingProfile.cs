using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Update;
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
            CreateMap<AppointmentInfoDto, Appointment>();
            CreateMap<Appointment, AppointmentInfoDto>();
            CreateMap<AppointmentTypeDto, AppointmentType>();
            CreateMap<AppointmentType, AppointmentTypeDto>();
            CreateMap<UpdateAppointmentTypeDto, AppointmentType>();
            CreateMap<AppointmentType, UpdateAppointmentTypeDto>();
            CreateMap<TreatmentDto, Treatment>();
            CreateMap<Treatment, TreatmentDto>();
            CreateMap<TreatmentPlanTemplate, TreatmentPlanTemplateDto>();
            CreateMap<TreatmentPlanTemplateItem, TreatmentPlanTemplateItemDto>();
            CreateMap<ClinicalSession, ClinicalSessionDto>();
            CreateMap<ClinicalSessionDto, ClinicalSession>();
            CreateMap<SessionPlanMaster, SessionPlanMasterDto>();
            CreateMap<SessionPlanMasterDto, SessionPlanMaster>();
            CreateMap<SessionPlanDetail, SessionPlanDetailDto>();
            CreateMap<SessionPlanDetailDto, SessionPlanDetail>();
            CreateMap<UpdateTreatmentDto, Treatment>();
            CreateMap<Treatment, UpdateTreatmentDto>();
            CreateMap<CurrencyDto, Currency>();
            CreateMap<Currency, CurrencyDto>();
            CreateMap<Payment, PaymentDto>();
            CreateMap<PaymentDto, Payment>();
            CreateMap<PaymentTermDto, PaymentTerm>();
            CreateMap<PaymentTerm, PaymentTermDto>();
            CreateMap<UpdatePaymentTermDto, PaymentTerm>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerInvoiceDashboardDto, Customer>();
            CreateMap<Customer, CustomerInvoiceDashboardDto>();
            //CreateMap<DoctorDto, Doctor>();
            CreateMap<Doctor, DoctorDto>();
            //CreateMap<UpdateDoctorDto, Doctor>();
            CreateMap<Doctor, UpdateDoctorDto>();
        }
    }
}
