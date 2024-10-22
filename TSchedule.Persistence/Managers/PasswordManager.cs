using TSchedule.Persistence.Interfaces.Managers;

namespace TSchedule.Persistence.Managers;

public class PasswordManager : IPasswordManager
{
    /// <summary>
    /// "Ленивое" создание сервиса
    /// </summary>
    private static readonly Lazy<PasswordManager> _instance = new(() => new PasswordManager());

    /// <summary>
    /// Ссылка на сервис по умолчанию
    /// </summary>
    public static PasswordManager Default => _instance.Value;

    /// <summary>
    /// Хэширует пароль
    /// </summary>
    /// <param name="password">Пароль в виде текста</param>
    /// <returns>Хэш пароля</returns>
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, 12);

    /// <summary>
    /// Проверяет пароль на соответствие
    /// </summary>
    /// <param name="text">Пароль в виде текста</param>
    /// <param name="hash">Пароль в виде хэша</param>
    /// <returns><b>true</b>, если пароль верный, иначе – <b>false</b></returns>
    public bool Verify(string text, string hash) => BCrypt.Net.BCrypt.Verify(text, hash);
}
