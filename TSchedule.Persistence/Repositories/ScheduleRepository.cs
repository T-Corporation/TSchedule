using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    public async Task<Schedule> FindById(int id)
    {
        await using ApplicationDbContext context = new();
        var schedule = await context.Schedules.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new EntityNotFoundException<Schedule>(nameof(id), id);
        return schedule;
    }

    public async Task<IEnumerable<Schedule>> FindAll()
    {
        await using ApplicationDbContext context = new();
        return await context.Schedules.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> FindWeeklyByTeacher(Guid teacherId, bool isDenominator = false)
    {
        await using ApplicationDbContext context = new();
        var query = context.Schedules.AsNoTracking()
            .Where(s => s.TeacherId == teacherId);

        if (isDenominator)
            query = query.Where(s => s.IsDenominator == isDenominator);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> FindWeeklyByStudentGroup(string groupCode, bool isDenominator = false)
    {
        await using ApplicationDbContext context = new();
        var query = context.Schedules.AsNoTracking()
            .Where(s => s.GroupCode == groupCode);

        if (isDenominator)
            query = query.Where(s => s.IsDenominator == isDenominator);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> FindWeeklyByYearAndSemester(short year, byte semester, bool isDenominator = false)
    {
        await using ApplicationDbContext context = new();
        var query = context.Schedules.AsNoTracking()
            .Where(s => s.Year == year && s.Semester == semester);

        if (isDenominator)
            query = query.Where(s => s.IsDenominator == isDenominator);

        return await query.ToListAsync();
    }

    public async Task<bool> Create(Schedule schedule, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new();
            await context.Schedules.AddAsync(schedule);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (willThrow) throw;
            return false;
        }
    }

    public async Task<bool> Update(Schedule schedule, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new();
            var existingSchedule = await context.Schedules.FirstOrDefaultAsync(s => s.Id == schedule.Id)
                ?? throw new EntityNotFoundException<Schedule>(nameof(schedule.Id), schedule.Id);

            context.Entry(existingSchedule).CurrentValues.SetValues(schedule);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            if (willThrow) throw;
            return false;
        }
    }

    public async Task<bool> DeleteById(int id, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new();
            var schedule = await context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null) throw new EntityNotFoundException<Schedule>(nameof(id), id);

            context.Schedules.Remove(schedule);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            if (willThrow) throw;
            return false;
        }
    }

    public async Task<bool> AddLesson(Schedule schedule)
    {
        try
        {
            await using ApplicationDbContext context = new();
            await context.Schedules.AddAsync(schedule);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> MoveLesson(int id, TimeSpan newStartTime, TimeSpan newEndTime, int classroomId)
    {
        try
        {
            await using ApplicationDbContext context = new();
            var lesson = await context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            if (lesson == null) throw new EntityNotFoundException<Schedule>(nameof(id), id);

            lesson.StartTime = newStartTime;
            lesson.EndTime = newEndTime;
            lesson.ClassroomId = classroomId;

            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> RemoveLesson(int id)
    {
        try
        {
            await using ApplicationDbContext context = new();
            var lesson = await context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            if (lesson == null) throw new EntityNotFoundException<Schedule>(nameof(id), id);

            context.Schedules.Remove(lesson);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public async Task<IEnumerable<Schedule>> GetAvailableTimeSlots(Guid teacherId, string groupCode, int classroomId, short year, byte semester)
    {
        await using ApplicationDbContext context = new();
        var schedules = await context.Schedules.AsNoTracking()
            .Where(s => s.TeacherId == teacherId && s.GroupCode == groupCode && s.ClassroomId == classroomId
                        && s.Year == year && s.Semester == semester)
            .ToListAsync();

        return schedules;
    }
}
