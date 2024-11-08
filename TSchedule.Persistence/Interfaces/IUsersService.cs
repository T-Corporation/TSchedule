using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IUsersService : IService
{
    Task Authenticate(string username, string password, Role role);
    Task AuthenticateById(Guid id, Role role);
    Task<bool> Register(ApplicationUser user);
    Task UpdateAccount(ApplicationUser user);
    Task DeleteAccount();

    bool IsAuthenticated();
    void Logout();
    Guid GetId();
    string GetUserName();
    string GetFullName();
    string GetPasswordHash();
    string GetEmail();
    string GetPhoneNumber();
    Role GetRole();
}
