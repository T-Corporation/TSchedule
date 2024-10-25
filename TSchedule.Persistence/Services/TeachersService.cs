using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class TeachersService : ITeachersService
{
    public async Task<bool> AddTeacher(Teacher teacher, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new();
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            if (willThrow) throw;
            return false;
        }        
    }

    public async Task<Teacher> GetTeacherByEmail(string email)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Email == email)
            ?? throw new EntityNotFoundException<Teacher>(nameof(email), email);
    }

    public async Task<Teacher> GetTeacherByPhoneNumber(string phoneNumber)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(t => t.PhoneNumber == phoneNumber)
            ?? throw new EntityNotFoundException<Teacher>(nameof(phoneNumber), phoneNumber);
    }

    public async Task<Teacher> GetTeacherByUserName(string userName)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(t => t.UserName == userName)
            ?? throw new EntityNotFoundException<Teacher>(nameof(userName), userName);
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachers()
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachersByPreferredTime(TimeOnly preferredTime)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .Where(t => t.PreferredTimeStart >= preferredTime && t.PreferredTimeEnd <= preferredTime)
            .ToListAsync();
    }

    public async Task<Teacher> GetTeacherById(Guid id)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new EntityNotFoundException<Teacher>(nameof(id), id);
    }

    public async Task<bool> IsTeacherPreferredTime(Guid id, TimeOnly preferredTime)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Id == id && t.PreferredTimeStart >= preferredTime && t.PreferredTimeEnd <= preferredTime) is not null;
    }

    public async Task<bool> RemoveTeacher(Guid id, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new();
            var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.Id == id)
                ?? throw new EntityNotFoundException<Teacher>(nameof(id), id);
            teacher.IsDeleted = true;
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            if (willThrow) throw;
            return false;
        }
    }

    public async Task<bool> UpdateTeacher(Teacher teacher, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new();
            var foundTeacher = await context.Teachers.FirstOrDefaultAsync(t => t.Id == teacher.Id)
                ?? throw new EntityNotFoundException<Teacher>(nameof(teacher.Id), teacher.Id);
            context.Entry(foundTeacher).CurrentValues.SetValues(teacher);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            if (willThrow) throw;
            return false;
        }
    }
}
