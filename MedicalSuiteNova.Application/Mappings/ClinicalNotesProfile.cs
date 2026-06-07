using AutoMapper;
using MedicalSuiteNova.Domain.Dto.ClinicalNotes;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class ClinicalNotesProfile : Profile
    {
        public ClinicalNotesProfile()
        {
            CreateMap<ClinicalNotesDto, ClinicalNotes>();
            CreateMap<ClinicalNotes, ClinicalNotesDto>();
            CreateMap<CreateClinicalNotesDto, ClinicalNotes>();
            CreateMap<ClinicalNotes, CreateClinicalNotesDto>();
            CreateMap<UpdateClinicalNotesDto, ClinicalNotes>();
            CreateMap<ClinicalNotes, UpdateClinicalNotesDto>();
        }
    }
}
