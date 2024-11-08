using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class ClassroomsService(string connectionString) : IClassroomsService
{
    public async Task AddClassroom(Classroom classroom)
    {
        await using ApplicationDbContext context = new(connectionString);
        if (await context.Classrooms.AnyAsync(c => c.Number == classroom.Number))
            throw new UniqueException($"Аудитория с номером {classroom.Number} уже существует");
        await context.Classrooms.AddAsync(classroom);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Classroom>> GetAllClassrooms()
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Classrooms.AsNoTracking()
            .ToListAsync();
    }

    public async Task<Classroom> GetClassroomById(int id)
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Classrooms.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new EntityNotFoundException<Classroom>(nameof(id), id);
    }

    public async Task RemoveClassroom(int id)
    {
        await using ApplicationDbContext context = new(connectionString);
        var classroom = await context.Classrooms.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new EntityNotFoundException<Classroom>(nameof(id), id);
        context.Classrooms.Remove(classroom);
        await context.SaveChangesAsync();
    }

    public async Task UpdateClassroom(Classroom classroom)
    {
        try
        {
            await using ApplicationDbContext context = new(connectionString);
            var foundClassroom = await context.Classrooms.FirstOrDefaultAsync(
                c => c.Id == classroom.Id)
                ?? throw new EntityNotFoundException<Classroom>(nameof(classroom.Id), classroom.Id);
            foundClassroom.Number = classroom.Number;
            foundClassroom.Type = classroom.Type;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new UniqueException($"Аудитория с номером {classroom.Number} уже существует");
        }
    }
}
