using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Repositories;

public class UsersRepository() : IUsersRepository
{
    public async Task<bool> Create(IUser user, bool willThrow = false)
    {
        try
        {
            using ApplicationDbContext context = new();
            await context.Teachers.AddAsync(user);
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
            using ApplicationDbContext context = new();
            var teacher = await context.Teachers.FirstOrDefaultAsync(
                t => t.Id == id) ?? throw new UserNotFoundException(nameof(id), id);

            context.Teachers.Remove(teacher);
            await context.SaveChangesAsync();
            return true;
        }
        catch (UserNotFoundException unfe)
        {
            Debug.WriteLine(unfe.Message);
            if (willThrow) throw;
            return false;
        }
    }

    public async Task<IEnumerable<IUser>> FindAll()
    {
        using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking() // Освобождение ресурсов, но с условием, что изменения не будут ослеживаться (update не будет работать)
            .ToListAsync();
    }

    public async Task<IEnumerable<IUser>> FindAllByFullName(string fio)
    {
        using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .Where(teacher => EF.Functions.Like(teacher.FullName, $"%{fio}%"))
            .ToListAsync();
    }

    public async Task<IUser> FindByEmail(string email)
    {
        using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                teacher => teacher.Email == email) ?? throw new UserNotFoundException(nameof(email), email);
    }

    public async Task<IUser> FindById(Guid id)
    {
        using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                teacher => teacher.Id == id) ?? throw new UserNotFoundException(nameof(id), id);
    }

    public async Task<IUser> FindByPhoneNumber(string phoneNumber)
    {
        using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                teacher => teacher.PhoneNumber == phoneNumber) ?? throw new UserNotFoundException(nameof(phoneNumber), phoneNumber);
    }

    public async Task<IUser> FindByUserName(string userName)
    {
        using ApplicationDbContext context = new();
        return await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(
                teacher => teacher.UserName == userName) ?? throw new UserNotFoundException(nameof(userName), userName);
    }

    public async Task<bool> Update(IUser user, bool willThrow = false)
    {
        try
        {
            using ApplicationDbContext context = new();
            var foundTeacher = await context.Teachers.FirstOrDefaultAsync(
                t => t.Id == user.Id) ?? throw new UserNotFoundException("Id", user.Id);

            foreach (var property in user.GetType().GetProperties())
                // Устанавливаем свойство у найденного препода равным тому, что у переданного в кач-ве параметра метода
                property.SetValue(foundTeacher, property.GetValue(user));

            await context.SaveChangesAsync();
            return true;
        }
        catch (UserNotFoundException unfe)
        {
            Debug.WriteLine(unfe.Message);
            if (willThrow) throw;
            return false;
        }
    }
}
