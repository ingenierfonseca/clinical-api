
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class SessionPlanDetailRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<SessionPlanDetail>(context, mapper), ISessionPlanDetailRepository
    {
    }
}
