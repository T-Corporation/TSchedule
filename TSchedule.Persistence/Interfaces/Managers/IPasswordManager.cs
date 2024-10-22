using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces.Managers;

public interface IPasswordManager : IManager
{
    string HashPassword(string password);
    bool Verify(string text, string hash);
}
