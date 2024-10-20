using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    public async Task<bool> Create(ApplicationUser user, bool willThrow = false)
    {
        try
        {
            using ApplicationDbContext context = new();

            switch (user.Role)
            {
                case UserRole.Teacher:
                    await context.Teachers.AddAsync((Teacher)user);
                    break;

                case UserRole.Administrator:
                    await context.Administrators.AddAsync((Administrator)user);
                    break;

                default:
                    throw new RoleNotSupportedException(user.Role);
            }

            await context.SaveChangesAsync();
            return true;
        }
        catch (RoleNotSupportedException rnfe)
        {
            Debug.WriteLine(rnfe.Message);
            if (willThrow) throw;
            return false;
        }
    }

    public async Task<bool> DeleteById(Guid id, UserRole role, bool willThrow = false)
    {
        try
        {
            using ApplicationDbContext context = new();

            ApplicationUser? user = role switch
            {
                UserRole.Teacher =>
                    await context.Teachers.FirstOrDefaultAsync(teacher => teacher.Id == id),

                UserRole.Administrator =>
                    await context.Administrators.FirstOrDefaultAsync(admin => admin.Id == id),

                _ => throw new RoleNotSupportedException(role),
            };

            if (user is null)
                throw new UserNotFoundException(nameof(id), id);

            if (user.IsDeleted)
            {
                Debug.WriteLine($"Пользователь с ролью \"{role}\" и id \"{id}\" уже удалён");
                return true;
            }

            user.IsDeleted = true;
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

    public async Task<IEnumerable<ApplicationUser>> FindAll(UserRole role)
    {
        using ApplicationDbContext context = new();

        return role switch
        {
            UserRole.Teacher => await context.Teachers.AsNoTracking()
                .Where(teacher => !teacher.IsDeleted)
                .ToListAsync(),

            UserRole.Administrator => await context.Administrators.AsNoTracking()
                .Where(admin => !admin.IsDeleted)
                .ToListAsync(),

            _ => throw new RoleNotSupportedException(role),
        };
    }

    public async Task<IEnumerable<ApplicationUser>> FindAllByFullName(string fullName, UserRole role)
    {
        using ApplicationDbContext context = new();

        return role switch
        {
            UserRole.Teacher => await context.Teachers.AsNoTracking()
                .Where(teacher => !teacher.IsDeleted && EF.Functions.Like(teacher.FullName, $"%{fullName}%"))
                .ToListAsync(),

            UserRole.Administrator => await context.Administrators.AsNoTracking()
                .Where(admin => !admin.IsDeleted && EF.Functions.Like(admin.FullName, $"%{fullName}%"))
                .ToListAsync(),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<ApplicationUser> FindByEmail(string email, UserRole role)
    {
        using ApplicationDbContext context = new();

        return role switch
        {
            UserRole.Teacher => await context.Teachers.AsNoTracking()
                .FirstOrDefaultAsync(teacher => !teacher.IsDeleted && teacher.Email == email)
                ?? throw new UserNotFoundException(nameof(email), email),

            UserRole.Administrator => await context.Administrators.AsNoTracking()
                .FirstOrDefaultAsync(admin => !admin.IsDeleted && admin.Email == email)
                ?? throw new UserNotFoundException(nameof(email), email),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<ApplicationUser> FindById(Guid id, UserRole role)
    {
        using ApplicationDbContext context = new();

        return role switch
        {
            UserRole.Teacher => await context.Teachers.AsNoTracking()
                .FirstOrDefaultAsync(teacher => !teacher.IsDeleted && teacher.Id == id)
                ?? throw new UserNotFoundException(nameof(id), id),

            UserRole.Administrator => await context.Administrators.AsNoTracking()
                .FirstOrDefaultAsync(admin => !admin.IsDeleted && admin.Id == id)
                ?? throw new UserNotFoundException(nameof(id), id),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<ApplicationUser> FindByPhoneNumber(string phoneNumber, UserRole role)
    {
        using ApplicationDbContext context = new();

        return role switch
        {
            UserRole.Teacher => await context.Teachers.AsNoTracking()
                .FirstOrDefaultAsync(teacher => !teacher.IsDeleted && teacher.PhoneNumber == phoneNumber)
                ?? throw new UserNotFoundException(nameof(phoneNumber), phoneNumber),

            UserRole.Administrator => await context.Administrators.AsNoTracking()
                .FirstOrDefaultAsync(admin => !admin.IsDeleted && admin.PhoneNumber == phoneNumber)
                ?? throw new UserNotFoundException(nameof(phoneNumber), phoneNumber),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<ApplicationUser> FindByUserName(string userName, UserRole role)
    {
        using ApplicationDbContext context = new();

        return role switch
        {
            UserRole.Teacher => await context.Teachers.AsNoTracking()
                .FirstOrDefaultAsync(teacher => !teacher.IsDeleted && teacher.UserName == userName)
                ?? throw new UserNotFoundException(nameof(userName), userName),

            UserRole.Administrator => await context.Administrators.AsNoTracking()
                .FirstOrDefaultAsync(admin => !admin.IsDeleted && admin.UserName == userName)
                ?? throw new UserNotFoundException(nameof(userName), userName),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<bool> Update(ApplicationUser user, bool willThrow = false)
    {
        try
        {
            using ApplicationDbContext context = new();

            ApplicationUser? foundUser = user.Role switch
            {
                UserRole.Teacher => await context.Teachers.FirstOrDefaultAsync(
                    teacher => teacher.Id == user.Id),

                UserRole.Administrator => await context.Administrators.FirstOrDefaultAsync(
                    admin => admin.Id == user.Id),

                _ => throw new RoleNotSupportedException(user.Role),
            };

            if (foundUser is null)
                throw new UserNotFoundException("Id", user.Id);

            foreach (var property in foundUser.GetType().GetProperties())
            {
                if (property.Name is "Id" or "Role")
                    continue;

                // Устанавливаем свойство у найденного юзера
                property.SetValue(foundUser, property.GetValue(user));
            }

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
