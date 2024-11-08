using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Repositories;

public class UsersRepository(string connectionString) : IUsersRepository
{
    public async Task<bool> Create(ApplicationUser user, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new(connectionString);

            switch (user.Role)
            {
                case Role.Преподаватель:
                    await context.Teachers.AddAsync((Teacher)user);
                    break;

                case Role.Администратор:
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

    public async Task<bool> DeleteById(Guid id, Role role, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new(connectionString);

            ApplicationUser? user = role switch
            {
                Role.Преподаватель =>
                    await context.Teachers.FirstOrDefaultAsync(teacher => teacher.Id == id),

                Role.Администратор =>
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

    public async Task<IEnumerable<ApplicationUser>> FindAll(Role role)
    {
        await using ApplicationDbContext context = new(connectionString);

        return role switch
        {
            Role.Преподаватель => await context.Teachers.AsNoTracking()
                .Where(teacher => !teacher.IsDeleted)
                .ToListAsync(),

            Role.Администратор => await context.Administrators.AsNoTracking()
                .Where(admin => !admin.IsDeleted)
                .ToListAsync(),

            _ => throw new RoleNotSupportedException(role),
        };
    }

    public async Task<IEnumerable<ApplicationUser>> FindByLikeFullName(string fullName, Role role)
    {
        await using ApplicationDbContext context = new(connectionString);

        return role switch
        {
            Role.Преподаватель => await context.Teachers.AsNoTracking()
                .Where(teacher => !teacher.IsDeleted && EF.Functions.Like(teacher.FullName, $"%{fullName}%"))
                .ToListAsync(),

            Role.Администратор => await context.Administrators.AsNoTracking()
                .Where(admin => !admin.IsDeleted && EF.Functions.Like(admin.FullName, $"%{fullName}%"))
                .ToListAsync(),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<ApplicationUser> FindByEmail(string email, Role role)
    {
        await using ApplicationDbContext context = new(connectionString);

        return role switch
        {
            Role.Преподаватель => await context.Teachers.AsNoTracking()
                .FirstOrDefaultAsync(teacher => !teacher.IsDeleted && teacher.Email == email)
                ?? throw new UserNotFoundException(nameof(email), email),

            Role.Администратор => await context.Administrators.AsNoTracking()
                .FirstOrDefaultAsync(admin => !admin.IsDeleted && admin.Email == email)
                ?? throw new UserNotFoundException(nameof(email), email),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<ApplicationUser> FindById(Guid id, Role role)
    {
        await using ApplicationDbContext context = new(connectionString);

        return role switch
        {
            Role.Преподаватель => await context.Teachers.AsNoTracking()
                .FirstOrDefaultAsync(teacher => !teacher.IsDeleted && teacher.Id == id)
                ?? throw new UserNotFoundException(nameof(id), id),

            Role.Администратор => await context.Administrators.AsNoTracking()
                .FirstOrDefaultAsync(admin => !admin.IsDeleted && admin.Id == id)
                ?? throw new UserNotFoundException(nameof(id), id),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<ApplicationUser> FindByPhoneNumber(string phoneNumber, Role role)
    {
        await using ApplicationDbContext context = new(connectionString);

        return role switch
        {
            Role.Преподаватель => await context.Teachers.AsNoTracking()
                .FirstOrDefaultAsync(teacher => !teacher.IsDeleted && teacher.PhoneNumber == phoneNumber)
                ?? throw new UserNotFoundException(nameof(phoneNumber), phoneNumber),

            Role.Администратор => await context.Administrators.AsNoTracking()
                .FirstOrDefaultAsync(admin => !admin.IsDeleted && admin.PhoneNumber == phoneNumber)
                ?? throw new UserNotFoundException(nameof(phoneNumber), phoneNumber),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<ApplicationUser> FindByUserName(string userName, Role role)
    {
        await using ApplicationDbContext context = new(connectionString);

        return role switch
        {
            Role.Преподаватель => await context.Teachers.AsNoTracking()
                .FirstOrDefaultAsync(teacher => !teacher.IsDeleted && teacher.UserName == userName)
                ?? throw new UserNotFoundException(nameof(userName), userName),

            Role.Администратор => await context.Administrators.AsNoTracking()
                .FirstOrDefaultAsync(admin => !admin.IsDeleted && admin.UserName == userName)
                ?? throw new UserNotFoundException(nameof(userName), userName),

            _ => throw new RoleNotSupportedException(role)
        };
    }

    public async Task<bool> Update(ApplicationUser user, bool willThrow = false)
    {
        try
        {
            await using ApplicationDbContext context = new(connectionString);

            ApplicationUser? existingUser = user.Role switch
            {
                Role.Преподаватель => await context.Teachers.FirstOrDefaultAsync(
                    teacher => teacher.Id == user.Id),

                Role.Администратор => await context.Administrators.FirstOrDefaultAsync(
                    admin => admin.Id == user.Id),

                _ => throw new RoleNotSupportedException(user.Role),
            };

            if (existingUser is null)
                throw new UserNotFoundException(nameof(user.Id), user.Id);

            if (user.Role is Role.Преподаватель)
                context.Teachers.Update((Teacher)existingUser);
            else if (user.Role is Role.Администратор)
                context.Administrators.Update((Administrator)existingUser);

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
