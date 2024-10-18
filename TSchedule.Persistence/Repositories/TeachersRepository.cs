using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Repositories;

public class TeachersRepository(ApplicationDbContext context) : ITeachersRepository
{
    public async Task<bool> Create(Teacher teacher, bool willThrow = false)
    {
        try
        {
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();
            return true;
        }
        catch (OperationCanceledException oce)
        {
            Debug.WriteLine(oce.Message);
            if (willThrow) throw;
            return false;
        }
    }

    public async Task<bool> DeleteById(Guid id, bool willThrow = false)
    {
        try
        {
            var teacher = await context.Teachers.FirstOrDefaultAsync(
                t => t.Id == id) ?? throw new TeacherNotFoundException(nameof(id), id);

            context.Teachers.Remove(teacher);
            await context.SaveChangesAsync();
            return true;
        }
        catch (TeacherNotFoundException tnfe)
        {
            Debug.WriteLine(tnfe.Message);
            if (willThrow) throw;
            return false;
        }
    }

    public async Task<IEnumerable<Teacher>> FindAll()
        => await context.Teachers.AsNoTracking() // Освобождение ресурсов, но с условием, что изменения не будут ослеживаться (update не будет работать)
            .ToListAsync();

    public async Task<IEnumerable<Teacher>> FindAllByFio(string fio)
        => await context.Teachers.AsNoTracking()
            .Where(
                teacher => EF.Functions.Like(teacher.Surname, $"%{fio}%")
                    || EF.Functions.Like(teacher.Name, $"%{fio}%")
                    || EF.Functions.Like(teacher.FatherName, $"%{fio}%")).ToListAsync();

    public async Task<Teacher> FindByEmail(string email)
        => await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                teacher => teacher.Email == email) ?? throw new TeacherNotFoundException(nameof(email), email);

    public async Task<Teacher> FindById(Guid id)
        => await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                teacher => teacher.Id == id) ?? throw new TeacherNotFoundException(nameof(id), id);

    public async Task<Teacher> FindByPhoneNumber(string phoneNumber)
        => await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                teacher => teacher.PhoneNumber == phoneNumber) ?? throw new TeacherNotFoundException(nameof(phoneNumber), phoneNumber);

    public async Task<Teacher> FindByUserName(string userName)
        => await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                teacher => teacher.UserName == userName) ?? throw new TeacherNotFoundException(nameof(userName), userName);

    public async Task<bool> Update(Teacher teacher, bool willThrow = false)
    {
        try
        {
            var foundTeacher = await FindById(teacher.Id);
            foreach (var property in teacher.GetType().GetProperties())
                // Устанавливаем свойство у найденного препода равным тому, что у переданного в кач-ве параметра метода
                property.SetValue(foundTeacher, property.GetValue(teacher));

            await context.SaveChangesAsync();
            return true;
        }
        catch (TeacherNotFoundException tnfe)
        {
            Debug.WriteLine(tnfe.Message);
            if (willThrow) throw;
            return false;
        }
    }
}
