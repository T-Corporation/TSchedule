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

    public async Task Authenticate(string username, string password, Role role)
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

    public async Task AuthenticateById(Guid id, Role role)
    {
        try
        {
            var user = await repository.FindById(id, role);
            ApplicationUser = user;
        }
        catch (UserNotFoundException unfe)
        {
            Debug.WriteLine(unfe.Message);
        }
    }

    public async Task<bool> Register(ApplicationUser user) => await repository.Create(user);

    public void Logout() => ApplicationUser = null;

    public bool IsAuthenticated() => ApplicationUser is not null;

    public string GetUserName() => ApplicationUser?.UserName ?? "Гость";

    public string GetFullName() => ApplicationUser?.FullName ?? "Гость";

    public Role GetRole() => ApplicationUser?.Role ?? Role.Гость;

    public Guid GetId() => ApplicationUser?.Id ?? Guid.Empty;

    public string GetPasswordHash() => ApplicationUser?.PasswordHash ?? string.Empty;

    public string GetEmail() => ApplicationUser?.Email ?? string.Empty;

    public string GetPhoneNumber() => ApplicationUser?.PhoneNumber ?? string.Empty;

    public async Task DeleteAccount() => await repository.DeleteById(GetId(), GetRole());

    public async Task UpdateAccount(ApplicationUser user) => await repository.Update(user);
}
