using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface ITeachersService : IService
{
    Task<Teacher> GetTeacherById(Guid id);
    Task<IEnumerable<Teacher>> GetAllTeachers();
    Task<Teacher> GetTeacherByEmail(string email);
    Task<Teacher> GetTeacherBySubjectId(int subjectId);
    Task<Teacher> GetTeacherByUserName(string userName);
    Task<Teacher> GetTeacherByPhoneNumber(string phoneNumber);

    Task AddTeacherWithPreferredTimes(Teacher teacher, IEnumerable<TeacherPreferredTime> preferredTimes);
    Task UpdateTeacherWithPreferredTimes(Teacher teacher, IEnumerable<TeacherPreferredTime> preferredTimes);
    Task RemoveTeacherWithPreferredTimes(Guid id);
}
