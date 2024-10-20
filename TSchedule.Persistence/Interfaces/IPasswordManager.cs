namespace TSchedule.Persistence.Interfaces;

public interface IPasswordManager : IManager
{
    string HashPassword(string password);
    bool Verify(string text, string hash);
}
