using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;

namespace TSchedule.Persistence.Services;

public class TeachersService : ITeachersService
{
    public async Task AddTeacherWithPreferredTimes(Teacher teacher, IEnumerable<TeacherPreferredTime> preferredTimes)
    {
        await using ApplicationDbContext context = new();
        if (await context.Teachers.AnyAsync(t => !t.IsDeleted && t.UserName == teacher.UserName))
            throw new UniqueException($"Преподаватель с именем для входа \"{teacher.UserName}\" уже существует");

        teacher.PasswordHash = PasswordManager.Default.HashPassword(teacher.PasswordHash);
        await context.Teachers.AddAsync(teacher);
        foreach (var preferredTime in preferredTimes)
            preferredTime.TeacherId = teacher.Id;
        await context.TeacherPreferredTimes.AddRangeAsync(preferredTimes);
        await context.SaveChangesAsync();
    }

    public async Task<Teacher> GetTeacherByEmail(string email)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .Include(t => t.Classroom)
            .Include(t => t.Subject)
                .ThenInclude(s => s!.Specialty)
            .Include(t => t.PreferredTimes)
            .FirstOrDefaultAsync(t => t.Email == email)
            ?? throw new EntityNotFoundException<Teacher>(nameof(email), email);
    }

    public async Task<Teacher> GetTeacherByPhoneNumber(string phoneNumber)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .Include(t => t.Classroom)
            .Include(t => t.Subject)
                .ThenInclude(s => s!.Specialty)
            .Include(t => t.PreferredTimes)
            .FirstOrDefaultAsync(t => t.PhoneNumber == phoneNumber)
            ?? throw new EntityNotFoundException<Teacher>(nameof(phoneNumber), phoneNumber);
    }

    public async Task<Teacher> GetTeacherByUserName(string userName)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .Include(t => t.Classroom)
            .Include(t => t.Subject)
                .ThenInclude(s => s!.Specialty)
            .Include(t => t.PreferredTimes)
            .FirstOrDefaultAsync(t => t.UserName == userName)
            ?? throw new EntityNotFoundException<Teacher>(nameof(userName), userName);
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachers()
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Include(t => t.Classroom)
            .Include(t => t.Subject)
                .ThenInclude(s => s!.Specialty)
            .Include(t => t.PreferredTimes)
            .ToListAsync();
    }

    public async Task<Teacher> GetTeacherById(Guid id)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .Include(t => t.Classroom)
            .Include(t => t.Subject)
                .ThenInclude(s => s!.Specialty)
            .Include(t => t.PreferredTimes)
            .FirstOrDefaultAsync(t => !t.IsDeleted && t.Id == id)
            ?? throw new EntityNotFoundException<Teacher>(nameof(id), id);
    }

    public async Task RemoveTeacherWithPreferredTimes(Guid id)
    {
        await using ApplicationDbContext context = new();
        var teacher = await context.Teachers
            .Include(t => t.Classroom)
            .Include(t => t.Subject)
                .ThenInclude(s => s!.Specialty)
            .Include(t => t.PreferredTimes)
            .FirstOrDefaultAsync(t => !t.IsDeleted && t.Id == id)
            ?? throw new EntityNotFoundException<Teacher>(nameof(id), id);

        teacher.IsDeleted = true;
        context.TeacherPreferredTimes.RemoveRange(teacher.PreferredTimes);

        await context.SaveChangesAsync();
    }

    public async Task UpdateTeacherWithPreferredTimes(Teacher teacher, IEnumerable<TeacherPreferredTime> preferredTimes)
    {
        try
        {
            await using ApplicationDbContext context = new();

            var existingTeacher = await context.Teachers
                .Include(t => t.Classroom)
                .Include(t => t.Subject)
                    .ThenInclude(s => s!.Specialty)
                .Include(t => t.PreferredTimes)
                .FirstOrDefaultAsync(t => !t.IsDeleted && t.Id == teacher.Id)
                ?? throw new EntityNotFoundException<Teacher>(nameof(teacher.Id), teacher.Id);

            existingTeacher.UserName = teacher.UserName;
            existingTeacher.FullName = teacher.FullName;
            existingTeacher.Email = teacher.Email;
            existingTeacher.PhoneNumber = teacher.PhoneNumber;
            existingTeacher.ClassroomId = teacher.ClassroomId;
            existingTeacher.SubjectId = teacher.SubjectId;
            existingTeacher.DateOfBirth = teacher.DateOfBirth;

            if (!PasswordManager.Default.IsHash(teacher.PasswordHash))
                existingTeacher.PasswordHash = PasswordManager.Default.HashPassword(teacher.PasswordHash);

            // Удаление старых предпочтительных времен и добавление новых
            context.TeacherPreferredTimes.RemoveRange(existingTeacher.PreferredTimes);
            await context.TeacherPreferredTimes.AddRangeAsync(preferredTimes);
        
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new UniqueException($"Преподаватель с именем для входа \"{teacher.UserName}\" уже существует");
        }
    }

    public async Task<Teacher> GetTeacherBySubjectId(int subjectId)
    {
        await using ApplicationDbContext context = new();
        return await context.Teachers
                .Include(t => t.Classroom)
                .Include(t => t.Subject)
                    .ThenInclude(s => s!.Specialty)
                .Include(t => t.PreferredTimes)
                .FirstOrDefaultAsync(t => t.SubjectId == subjectId)
            ?? throw new EntityNotFoundException<Teacher>(nameof(subjectId), subjectId);
    }
}
