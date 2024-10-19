using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class PasswordService : IPasswordService
{
    public static PasswordService Default { get; set; } = new();

    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, 12);

    public bool Verify(string text, string hash) => BCrypt.Net.BCrypt.Verify(text, hash);
}
