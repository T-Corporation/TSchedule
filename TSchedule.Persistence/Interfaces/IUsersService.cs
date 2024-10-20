using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Interfaces;

public interface IUsersService
{
    Task<IUser?> Authenticate(string username, string password);
    Task<bool> Register(IUser user);
}
