namespace TSchedule.Persistence.Interfaces;

public interface IPasswordService
{
    string HashPassword(string password);
    bool Verify(string text, string hash);
}
