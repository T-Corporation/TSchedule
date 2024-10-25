using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class ScheduleService(IScheduleRepository repository)
{
    // Проверка корректности расписания
    public async Task<bool> CheckScheduleConflict(Guid teacherId, TimeSpan startTime, TimeSpan endTime)
    {
        var schedules = await repository.FindWeeklyByTeacher(teacherId);

        // Проверяем пересечение времени
        foreach (var schedule in schedules)
            if ((startTime >= schedule.StartTime && startTime < schedule.EndTime) ||
                (endTime > schedule.StartTime && endTime <= schedule.EndTime))
                return true; // Конфликт

        return false; // Конфликтов нет
    }

    // Проверка доступность аудитории
    public async Task<bool> CheckClassroomAvailability(int classroomId, TimeSpan startTime, TimeSpan endTime)
    {
        var schedules = await repository.FindAll();

        foreach (var schedule in schedules.Where(s => s.ClassroomId == classroomId))
            if ((startTime >= schedule.StartTime && startTime < schedule.EndTime) ||
                (endTime > schedule.StartTime && endTime <= schedule.EndTime))
                return false; // Аудитория занята

        return true; // Аудитория свободна
    }
}
