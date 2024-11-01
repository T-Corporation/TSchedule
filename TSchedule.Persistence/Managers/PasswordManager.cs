using System.Text.RegularExpressions;
using TSchedule.Persistence.Interfaces.Managers;

namespace TSchedule.Persistence.Managers;

public partial class PasswordManager : IPasswordManager
{
    public const int MinPasswordLength = 8;

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

    /// <summary>
    /// Проверяет, является ли текст хешем
    /// </summary>
    /// <param name="text">Текст</param>
    /// <returns><b>true</b>, если хеш, иначе - <b>false</b></returns>
    public bool IsHash(string text)
    {
        // Проверка на пустую строку
        if (string.IsNullOrWhiteSpace(text))
            return false;

        // Проверка на длину хэша BCrypt (60 символов)
        if (text.Length != 60)
            return false;

        // Проверка на наличие префикса BCrypt
        if (!text.StartsWith("$2a$") && !text.StartsWith("$2b$") && !text.StartsWith("$2y$"))
            return false;

        return true;
    }

    /// <summary>
    /// Проверяет безопасность пароля
    /// </summary>
    /// <param name="password">Пароль для проверки</param>
    /// <returns><b>true</b>, если пароль безопасен, иначе – <b>false</b></returns>
    public bool IsPasswordSafe(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < MinPasswordLength)
            return false;

        // Проверка на наличие хотя бы одной заглавной буквы
        if (!LatinUpperCaseLettersRegex().IsMatch(password))
            return false;

        // Проверка на наличие хотя бы одной строчной буквы
        if (!LatinLowerCaseLettersRegex().IsMatch(password))
            return false;

        // Проверка на наличие хотя бы одной цифры
        if (!NumbersRegex().IsMatch(password))
            return false;

        // Проверка на наличие хотя бы одного специального символа
        if (!SpecialCharactersRegex().IsMatch(password))
            return false;

        return true;
    }

    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex LatinUpperCaseLettersRegex();

    [GeneratedRegex(@"[a-z]")]
    private static partial Regex LatinLowerCaseLettersRegex();

    [GeneratedRegex(@"\d")]
    private static partial Regex NumbersRegex();

    [GeneratedRegex(@"[!@#$%^&*(),.?""':{}|<>]")]
    private static partial Regex SpecialCharactersRegex();
}
