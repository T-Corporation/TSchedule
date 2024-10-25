using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IScheduleRepository : IRepository
{
    Task<Schedule> FindById(int id);
    Task<IEnumerable<Schedule>> FindAll();
    Task<IEnumerable<Schedule>> FindWeeklyByTeacher(Guid teacherId, bool isDenominator = false);
    Task<IEnumerable<Schedule>> FindWeeklyByStudentGroup(string groupCode, bool isDenominator = false);
    Task<IEnumerable<Schedule>> FindWeeklyByYearAndSemester(short year, byte semester, bool isDenominator = false);

    Task<bool> Create(Schedule schedule, bool willThrow = false);
    Task<bool> Update(Schedule schedule, bool willThrow = false);
    Task<bool> DeleteById(int id, bool willThrow = false);

    // Новые методы для добавления и перемещения занятий
    Task<bool> AddLesson(Schedule schedule);
    Task<bool> MoveLesson(int id, TimeSpan newStartTime, TimeSpan newEndTime, int classroomId);
    Task<bool> RemoveLesson(int id);
    Task<IEnumerable<Schedule>> GetAvailableTimeSlots(Guid teacherId, string groupCode, int classroomId, short year, byte semester);
}
