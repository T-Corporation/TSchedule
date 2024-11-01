using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface ISubjectsService : IService
{
    Task<Subject> GetSubjectById(int id);
    Task<Subject> GetSubjectByCode(string code);
    Task<IEnumerable<Subject>> GetAllSubjects();
    Task<IEnumerable<Subject>> GetSubjectsBySpecialtyId(int id);
    Task<IEnumerable<Subject>> GetSubjectsByLikeQuery(string name);

    Task AddSubject(Subject subject);
    Task UpdateSubject(Subject subject);
    Task RemoveSubject(int id);
}
