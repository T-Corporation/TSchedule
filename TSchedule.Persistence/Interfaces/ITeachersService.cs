using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Interfaces;

public interface ITeachersService
{
    Task<bool> Authenticate(string username, string password);
    Task<bool> Register(Teacher teacher);
}
