using System.Text.RegularExpressions;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Managers;

public partial class EmailManager : IManager
{
    private static readonly Lazy<EmailManager> _instance = new(() => new EmailManager());
    public static EmailManager Default => _instance.Value;

    /// <summary>
    /// Популярные домены для проверки
    /// </summary>
    private static readonly HashSet<string> AllowedDomains =
    [
        "gmail.com", "yahoo.com", "outlook.com", "icloud.com", "mail.ru",
        "yandex.ru", "rambler.ru", "bk.ru", "hotmail.com"
    ];

    /// <summary>
    /// Проверяет корректность адреса электронной почты и его домена.
    /// </summary>
    /// <param name="email">Адрес электронной почты для проверки.</param>
    /// <returns><c>true</c>, если адрес электронной почты корректен и домен поддерживается, иначе <c>false</c>.</returns>
    public bool IsEmailValid(string email)
    {
        if (!EmailRegex().IsMatch(email)) return false;
        var domain = email.Split('@')[1];
        return AllowedDomains.Contains(domain);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailRegex();
}
