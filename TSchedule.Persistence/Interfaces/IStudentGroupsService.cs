using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IStudentGroupsService : IService
{
    Task<StudentGroup> GetStudentGroupByCode(string code);
    Task<IEnumerable<StudentGroup>> GetAllStudentGroups();
    Task<IEnumerable<StudentGroup>> GetAllStudentGroupsByCourse(byte course);
    Task<IEnumerable<StudentGroup>> GetAllStudentGroupsByGroupCode(string groupCode);
    Task<IEnumerable<StudentGroup>> GetAllStudentGroupsBySpecialtyCode(string specialtyCode);

    Task<bool> AddStudentGroup(StudentGroup group);
    Task<bool> UpdateStudentGroup(StudentGroup group);
    Task<bool> RemoveStudentGroup(string code);
}
