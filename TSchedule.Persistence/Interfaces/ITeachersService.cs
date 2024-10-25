using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface ITeachersService : IService
{
    Task<Teacher> GetTeacherById(Guid id);
    Task<IEnumerable<Teacher>> GetAllTeachers();
    Task<Teacher> GetTeacherByEmail(string email);
    Task<Teacher> GetTeacherByUserName(string userName);
    Task<Teacher> GetTeacherByPhoneNumber(string phoneNumber);
    Task<bool> IsTeacherPreferredTime(Guid id, TimeOnly preferredTime);
    Task<IEnumerable<Teacher>> GetAllTeachersByPreferredTime(TimeOnly preferredTime);

    Task<bool> AddTeacher(Teacher teacher, bool willThrow = false);
    Task<bool> UpdateTeacher(Teacher teacher, bool willThrow = false);
    Task<bool> RemoveTeacher(Guid id, bool willThrow = false);
}
