using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IGroupsService : IService
{
    Task<Group> GetGroupById(int id);
    Task<Group> GetGroupByCode(string code);
    Task<IEnumerable<Group>> GetAllGroups();
    Task<IEnumerable<Group>> GetAllGroupsByCourse(byte course);
    Task<IEnumerable<Group>> GetAllGroupsBySpecialtyId(int specialtyId);

    Task AddGroupAndSubjects(Group group, IEnumerable<Subject> subjects);
    Task UpdateGroupAndSubjects(Group group, IEnumerable<Subject> subjects);
    Task RemoveGroupAndSubjects(int id);
}
