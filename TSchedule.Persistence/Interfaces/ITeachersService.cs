using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface ITeachersService : IService
{
    Task<Teacher> GetTeacherById(Guid id);
    Task<IEnumerable<Teacher>> GetAllTeachers();
    Task<Teacher> GetAllTeacherByEmail(string email);
    Task<Teacher> GetAllTeacherByUserName(string userName);
    Task<Teacher> GetAllTeacherByPhoneNumber(string phoneNumber);
    Task<bool> IsTeacherPreferredTime(Guid id, TimeOnly preferredTime);
    Task<IEnumerable<Teacher>> GetAllTeachersByPreferredTime(TimeOnly preferredTime);

    Task<bool> AddTeacher(Teacher teacher);
    Task<bool> UpdateTeacher(Teacher teacher);
    Task<bool> RemoveTeacher(Guid id);
}
