using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class WeekDaysService(string connectionString) : IWeekDaysService
{
    public async Task<IEnumerable<Entities.WeekDay>> GetAllDaysOfWeek()
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.DaysOfWeek.AsNoTracking()
            .ToListAsync();
    }
}
