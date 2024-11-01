using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class SubjectsService : ISubjectsService
{
    public async Task AddSubject(Subject subject)
    {
        await using ApplicationDbContext context = new();
        if (await context.Subjects.AnyAsync(s => s.Code == subject.Code || s.Name == subject.Name))
            throw new UniqueException($"Предмет с таким кодом или названием уже существует");
        await context.Subjects.AddAsync(subject);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Subject>> GetAllSubjects()
    {
        await using ApplicationDbContext context = new();
        return await context.Subjects.AsNoTracking()
            .Include(s => s.Specialty)
            .ToListAsync();
    }

    public async Task<Subject> GetSubjectByCode(string code)
    {
        await using ApplicationDbContext context = new();
        return await context.Subjects.AsNoTracking()
            .Include(s => s.Specialty)
            .FirstOrDefaultAsync(s => s.Code == code)
            ?? throw new EntityNotFoundException<Subject>(nameof(code), code);
    }

    public async Task<Subject> GetSubjectById(int id)
    {
        await using ApplicationDbContext context = new();
        return await context.Subjects.AsNoTracking()
            .Include(s => s.Specialty)
            .FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new EntityNotFoundException<Subject>(nameof(id), id);
    }

    public async Task<IEnumerable<Subject>> GetSubjectsByLikeQuery(string query)
    {
        await using ApplicationDbContext context = new();
        return await context.Subjects.AsNoTracking()
            .Where(s => EF.Functions.Like(s.Code, $"%{query}%")
            || EF.Functions.Like(s.Name, $"%{query}%"))
            .Include(s => s.Specialty)
            .ToListAsync();
    }

    public async Task<IEnumerable<Subject>> GetSubjectsBySpecialtyId(int id)
    {
        await using ApplicationDbContext context = new();
        return await context.Subjects.AsNoTracking()
            .Where(s => s.SpecialtyId == id)
            .Include(s => s.Specialty)
            .ToListAsync();
    }

    public async Task RemoveSubject(int id)
    {
        await using ApplicationDbContext context = new();
        var foundSubject = await context.Subjects
            .Include(s => s.Specialty)
            .FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new EntityNotFoundException<Subject>(nameof(id), id);
        context.Subjects.Remove(foundSubject);
        await context.SaveChangesAsync();
    }

    public async Task UpdateSubject(Subject subject)
    {
        try
        {
            await using ApplicationDbContext context = new();
            var foundSubject = await context.Subjects
                .Include(t => t.Specialty)
                .FirstOrDefaultAsync(s => s.Id == subject.Id)
                ?? throw new EntityNotFoundException<Subject>(nameof(subject.Id), subject.Id);
            foundSubject.Code = subject.Code;
            foundSubject.Name = subject.Name;
            foundSubject.SpecialtyId = subject.SpecialtyId;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new UniqueException($"Предмет с таким кодом или названием уже существует");
        }
    }
}
