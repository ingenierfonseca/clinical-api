
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class SessionPlanMasterRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<SessionPlanMaster>(context, mapper), ISessionPlanMasterRepository
    {
    }
}
