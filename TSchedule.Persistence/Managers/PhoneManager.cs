using System.Text.RegularExpressions;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Managers;

public partial class PhoneManager : IManager
{
    private static readonly Lazy<PhoneManager> _instance = new(() => new PhoneManager());
    public static PhoneManager Default => _instance.Value;

    /// <summary>
    /// Проверяет корректность номера телефона (только для формата: 7xxxxxxxxxx).
    /// </summary>
    /// <param name="phoneNumber">Номер телефона для проверки.</param>
    /// <returns><c>true</c>, если номер телефона корректен, иначе <c>false</c>.</returns>
    // ReSharper disable once MemberCanBeMadeStatic.Global
    #pragma warning disable CA1822
    public bool IsPhoneNumberValid(string phoneNumber)
    {
        return PhoneRegex().IsMatch(phoneNumber);
    }

    /*/// <summary>
    /// Возвращает номер телефона в красивом виде
    /// </summary>
    /// <param name="phoneNumber">Номер телефона в формате <code>7xxxxxxxxxx</code></param>
    /// <returns>Красивый номер телефона</returns>
    public string MakePhoneNumberPretty(string phoneNumber)
    {
        // Проверка корректности номера телефона
        if (!IsPhoneNumberValid(phoneNumber)) 
            return string.Empty;

        // Форматирование номера
        return $"+7 ({phoneNumber.Substring(1, 3)}) {phoneNumber.Substring(4, 3)}-{phoneNumber.Substring(7, 2)}-{phoneNumber.Substring(9, 2)}";
    }*/


    [GeneratedRegex(@"^7\d{10}$")]
    private static partial Regex PhoneRegex();
}
