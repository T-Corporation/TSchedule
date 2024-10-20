using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Interfaces;

public interface IUsersService : IService
{
    Task Authenticate(string username, string password, UserRole role);
    Task<bool> Register(ApplicationUser user);
    bool IsAuthenticated();
    void Logout();
}
