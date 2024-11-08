using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class SpecialtiesService(string connectionString) : ISpecialtiesService
{
    public async Task AddSpecialty(Specialty specialty)
    {
        await using ApplicationDbContext context = new(connectionString);
        if (await context.Specialties.AnyAsync(s => s.Code == specialty.Code))
            throw new UniqueException($"Специальность с кодом {specialty.Code} уже существует");
        await context.Specialties.AddAsync(specialty);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Specialty>> GetAllSpecialties()
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Specialties.AsNoTracking()
            .ToListAsync();
    }

    public async Task<Specialty> GetSpecialtyById(int id)
    {
        await using ApplicationDbContext context = new(connectionString);
        return await context.Specialties.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new EntityNotFoundException<Specialty>(nameof(id), id);
    }

    public async Task RemoveSpecialty(int id)
    {
        await using ApplicationDbContext context = new(connectionString);
        var foundSpecialty = await context.Specialties.FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new EntityNotFoundException<Specialty>(nameof(id), id);
        context.Specialties.Remove(foundSpecialty);
        await context.SaveChangesAsync();
    }

    public async Task UpdateSpecialty(Specialty specialty)
    {
        try
        {
            await using ApplicationDbContext context = new(connectionString);
            var foundSpecialty = await context.Specialties.FirstOrDefaultAsync(s => s.Id == specialty.Id)
                ?? throw new EntityNotFoundException<Specialty>(nameof(specialty.Id), specialty.Id);
            foundSpecialty.Code = specialty.Code;
            foundSpecialty.Name = specialty.Name;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new UniqueException($"Специальность с кодом {specialty.Code} уже существует");
        }
    }
}
