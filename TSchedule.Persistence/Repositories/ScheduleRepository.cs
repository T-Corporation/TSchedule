using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(bool isDenominator)
    {
        await using ApplicationDbContext context = new();
        return await context.Schedules
            .Where(s => s.IsDenominator == isDenominator)
            .Include(s => s.Teacher)
                .ThenInclude(t => t!.Classroom)
            .Include(s => s.Teacher)
                .ThenInclude(t => t!.Subject)
                    .ThenInclude(s => s!.Specialty)
            .Include(s => s.Teacher)
                .ThenInclude(t => t!.Subject)
                    .ThenInclude(s => s!.GroupSubjects)
            .Include(s => s.WeekDay)
            .Include(s => s.Group)
                .ThenInclude(g => g!.Specialty)
            .Include(s => s.Lesson)
            .ToListAsync();
    }

    public async Task<Schedule?> GetScheduleByIdAsync(int id)
    {
        await using ApplicationDbContext context = new();
        return await context.Schedules
            .Include(s => s.Teacher)
                .ThenInclude(t => t!.Classroom)
            .Include(s => s.Teacher)
                .ThenInclude(t => t!.Subject)
                    .ThenInclude(s => s!.Specialty)
            .Include(s => s.Teacher)
                .ThenInclude(t => t!.Subject)
                    .ThenInclude(s => s!.GroupSubjects)
            .Include(s => s.WeekDay)
            .Include(s => s.Group)
                .ThenInclude(g => g!.Specialty)
            .Include(s => s.Lesson)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Schedule> AddScheduleAsync(Schedule schedule)
    {
        await using ApplicationDbContext context = new();
        var newSchedule = await context.Schedules.AddAsync(schedule);
        await context.SaveChangesAsync();
        return newSchedule.Entity;
    }

    public async Task UpdateScheduleAsync(Schedule schedule)
    {
        await using ApplicationDbContext context = new();
        context.Schedules.Update(schedule);
        await context.SaveChangesAsync();
    }

    public async Task DeleteScheduleAsync(int id)
    {
        await using ApplicationDbContext context = new();
        var schedule = await context.Schedules.FindAsync(id);
        if (schedule is null) return;
        context.Schedules.Remove(schedule);
        await context.SaveChangesAsync();
    }
}
