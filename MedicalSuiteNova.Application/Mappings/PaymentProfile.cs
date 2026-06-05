
using AutoMapper;
using MedicalSuiteNova.Domain.Dto.Payment;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>();
            CreateMap<PaymentDto, Payment>();
            CreateMap<PaymentTermDto, PaymentTerm>();
            CreateMap<PaymentTerm, PaymentTermDto>();
            CreateMap<UpdatePaymentTermDto, PaymentTerm>();
        }
    }
}
