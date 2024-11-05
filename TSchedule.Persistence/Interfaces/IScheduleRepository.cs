using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IScheduleRepository : IRepository
{
    Task<IEnumerable<Schedule>> GetSchedulesAsync(bool isDenominator);
    Task<Schedule?> GetScheduleByIdAsync(int id);
    Task<Schedule> AddScheduleAsync(Schedule schedule);
    Task UpdateScheduleAsync(Schedule schedule);
    Task DeleteScheduleAsync(int id);
}
