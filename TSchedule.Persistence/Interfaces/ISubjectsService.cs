using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface ISubjectsService : IService
{
    Task<Subject> GetSubjectByCode(string code);
    Task<IEnumerable<Subject>> GetAllSubjects();
    Task<IEnumerable<Subject>> GetSubjectsByLikeName(string name);
    Task<IEnumerable<Subject>> GetSubjectsBySpecialtyCode(string code);
    Task<bool> AddSubject(Subject subject);
    Task<bool> UpdateSubject(Subject subject);
    Task<bool> RemoveSubject(string code);
}
