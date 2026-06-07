using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.ClinicalNotes;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class ClinicalFileProfile : Profile
    {
        public ClinicalFileProfile()
        {
            CreateMap<ClinicalFileDto, ClinicalFile>();
            CreateMap<ClinicalFile, ClinicalFileDto>();
        }
    }
}
