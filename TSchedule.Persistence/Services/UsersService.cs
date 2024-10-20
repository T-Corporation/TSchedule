using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;

namespace TSchedule.Persistence.Services;

public class UsersService(IUsersRepository repository) : IUsersService
{
    private ApplicationUser? ApplicationUser { get; set; }

    public async Task Authenticate(string username, string password, Роль role)
    {
        try
        {
            var user = await repository.FindByUserName(username, role);

            if (!PasswordManager.Default.Verify(password, user.PasswordHash))
                throw new UserNotFoundException(
                    "UserName; Password; Role",
                    $"UserName={username}; Password={password}; Role={role}");

            ApplicationUser = user;
        }
        catch (UserNotFoundException unfe)
        {
            Debug.WriteLine(unfe.Message);
        }
    }

    public async Task<bool> Register(ApplicationUser user)
    {
        return await repository.Create(user);
    }

    public void Logout() => ApplicationUser = null;

    public bool IsAuthenticated() => ApplicationUser is not null;
}
