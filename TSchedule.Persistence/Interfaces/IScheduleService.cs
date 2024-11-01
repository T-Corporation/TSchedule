using TSchedule.Persistence.Interfaces.Bases;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Interfaces;

public interface IScheduleService : IService
{
    Task<IEnumerable<Schedule>> GetAllSchedules();
    Task<IEnumerable<Schedule>> GetSchedules(bool isDenominator);
    Task AddSchedule(Schedule schedule);
    Task RemoveSchedule(int id);
}

// Save & Delete vs Add & Remove...
