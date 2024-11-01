using TSchedule.Persistence.Interfaces.Bases;
using WeekDay = TSchedule.Persistence.Entities.WeekDay;

namespace TSchedule.Persistence.Interfaces;

public interface IWeekDaysService : IService
{
    Task<IEnumerable<WeekDay>> GetAllDaysOfWeek();
}
