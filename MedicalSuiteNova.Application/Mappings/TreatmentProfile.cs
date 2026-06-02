
using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class TreatmentProfile : Profile
    {
        public TreatmentProfile()
        {
            CreateMap<TreatmentDto, Treatment>();
            CreateMap<Treatment, TreatmentDto>();
            CreateMap<TreatmentCategoryDto, TreatmentCategory>();
            CreateMap<TreatmentCategory, TreatmentCategoryDto>();
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
        }
    }
}
