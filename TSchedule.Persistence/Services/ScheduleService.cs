using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class ScheduleService(IScheduleRepository repository) : IScheduleService
{
    public async Task<IEnumerable<Schedule>> GetAllSchedules()
    {
        var numeratorSchedules = await repository.GetSchedulesAsync(false);
        var denominatorSchedules = await repository.GetSchedulesAsync(true);
        return numeratorSchedules.Concat(denominatorSchedules);
    }

    public async Task<IEnumerable<Schedule>> GetSchedules(bool isDenominator)
        => await repository.GetSchedulesAsync(isDenominator);

    public async Task<Schedule> AddSchedule(Schedule schedule)
        => await repository.AddScheduleAsync(schedule);

    public async Task RemoveSchedule(int id)
        => await repository.DeleteScheduleAsync(id);

    public async Task UpdateSchedule(Schedule schedule)
        => await repository.UpdateScheduleAsync(schedule);
}
