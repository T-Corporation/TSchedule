using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class GroupsService : IGroupsService
{
    public async Task AddGroupAndSubjects(Group group, IEnumerable<Subject> subjects)
    {
        await using ApplicationDbContext context = new();
        
        if (await context.Groups.AnyAsync(g => g.Code == group.Code))
            throw new UniqueException($"Группа с кодом {group.Code} уже существует");

        await context.Groups.AddAsync(group);
        await context.SaveChangesAsync();

        // Добавление связей с предметами
        foreach (var subject in subjects)
            await context.GroupSubjects.AddAsync(new GroupSubject { GroupId = group.Id, SubjectId = subject.Id });
        
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Group>> GetAllGroups()
    {
        await using ApplicationDbContext context = new();
        
        return await context.Groups
            .AsNoTracking()
            .Include(g => g.Specialty)
            .Include(g => g.GroupSubjects)
                .ThenInclude(gs => gs.Subject)
            .ToListAsync();
    }

    public async Task<IEnumerable<Group>> GetAllGroupsByCourse(byte course)
    {
        await using ApplicationDbContext context = new();
        
        return await context.Groups
            .AsNoTracking()
            .Include(g => g.Specialty)
            .Include(g => g.GroupSubjects)
                .ThenInclude(gs => gs.Subject)
            .Where(g => g.Course == course)
            .ToListAsync();
    }

    public async Task<Group> GetGroupById(int id)
    {
        await using ApplicationDbContext context = new();
        
        return await context.Groups
            .AsNoTracking()
            .Include(g => g.Specialty)
            .Include(g => g.GroupSubjects)
                .ThenInclude(gs => gs.Subject)
            .FirstOrDefaultAsync(g => g.Id == id)
            ?? throw new EntityNotFoundException<Group>(nameof(id), id);
    }

    public async Task<IEnumerable<Group>> GetAllGroupsBySpecialtyId(int specialtyId)
    {
        await using ApplicationDbContext context = new();
        
        return await context.Groups
            .AsNoTracking()
            .Include(g => g.Specialty)
            .Include(g => g.GroupSubjects)
                .ThenInclude(gs => gs.Subject)
            .Where(g => g.SpecialtyId == specialtyId)
            .ToListAsync();
    }

    public async Task<Group> GetGroupByCode(string code)
    {
        await using ApplicationDbContext context = new();
        
        return await context.Groups
            .AsNoTracking()
            .Include(g => g.Specialty)
            .Include(g => g.GroupSubjects)
                .ThenInclude(gs => gs.Subject)
            .FirstOrDefaultAsync(g => g.Code == code)
            ?? throw new EntityNotFoundException<Group>(nameof(code), code);
    }

    public async Task RemoveGroupAndSubjects(int id)
    {
        await using ApplicationDbContext context = new();
        
        var group = await context.Groups
            .Include(g => g.GroupSubjects)
                .ThenInclude(gs => gs.Subject)
            .FirstOrDefaultAsync(g => g.Id == id)
            ?? throw new EntityNotFoundException<Group>(nameof(id), id);

        context.GroupSubjects.RemoveRange(group.GroupSubjects);
        context.Groups.Remove(group);

        await context.SaveChangesAsync();
    }

    public async Task UpdateGroupAndSubjects(Group group, IEnumerable<Subject> subjects)
    {
        try
        {
            await using ApplicationDbContext context = new();
        
            var foundGroup = await context.Groups
                .Include(g => g.GroupSubjects)
                    .ThenInclude(gs => gs.Subject)
                        .ThenInclude(s => s.Specialty)
                .FirstOrDefaultAsync(g => g.Id == group.Id)
                ?? throw new EntityNotFoundException<Group>(nameof(group.Id), group.Id);

            foundGroup.Code = group.Code;
            foundGroup.Course = group.Course;
            foundGroup.SpecialtyId = group.SpecialtyId;

            await context.GroupSubjects.ToListAsync();

            // Обновление предметов группы
            context.GroupSubjects.RemoveRange(foundGroup.GroupSubjects);
            foreach (var subject in subjects)
                context.GroupSubjects.Add(new GroupSubject { GroupId = group.Id, SubjectId = subject.Id });

            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new UniqueException($"Группа с кодом {group.Code} уже существует");
        }
    }
}
