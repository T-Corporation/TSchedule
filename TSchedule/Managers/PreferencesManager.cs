using System.Diagnostics;
using System.Runtime.InteropServices;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces.Bases;
using TSchedule.Properties;

namespace TSchedule.Managers;

public partial class PreferencesManager : IManager
{
    #region System Api

    [return: MarshalAs(UnmanagedType.Bool)]
    [LibraryImport("UXTheme.dll", EntryPoint = "#138", SetLastError = true)]
    private static partial bool ShouldSystemUseDarkMode();

    public bool IsDarkModeEnabled => ShouldSystemUseDarkMode();

    #endregion

    #region Static fields

    /// <summary>
    /// Словарь заводских настроек
    /// </summary>
    private static readonly Dictionary<string, dynamic> _defaultSettings = new()
    {
        { "UserGuid", Guid.Empty },
        { "Role", Роль.Гость.ToString() },
        { "IsLoggedIn", false },
        { "Theme", "System" },
        { "FontFamily", "Segoe UI" },
        { "UpdateRate", TimeSpan.FromMinutes(30) }
    };

    /// <summary>
    /// Доступные темы
    /// </summary>
    private static readonly string[] _themes = ["Dark", "Light", "System"];

    /// <summary>
    /// Доступные роли
    /// </summary>
    private static readonly string[] _roles = [Роль.Гость.ToString(), Роль.Преподаватель.ToString(), Роль.Администратор.ToString()];

    /// <summary>
    /// "Ленивая" инициализация менеджера
    /// </summary>
    private static readonly Lazy<PreferencesManager> _instance = new(() => new PreferencesManager());

    #endregion

    #region Properties

    /// <summary>
    /// Ссылка на менеджер по умолчанию
    /// </summary>
    public static PreferencesManager Default => _instance.Value;

    #endregion

    #region Getters, Setters and Clearings

    // UserGuid

    /// <summary>
    /// Получает Guid авторизованного пользователя
    /// </summary>
    /// <returns>Guid</returns>
    public Guid GetUserGuid() => Preferences.Default.UserGuid;

    /// <summary>
    /// Устанавливает Guid авторизованного пользователя
    /// </summary>
    /// <param name="guid">Guid</param>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager SetUserGuid(Guid guid)
    {
        Preferences.Default.UserGuid = guid;
        return this;
    }

    /// <summary>
    /// Очистка Guid авторизованного пользователя
    /// </summary>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager ClearUserGuid()
    {
        Preferences.Default.UserGuid = _defaultSettings["UserGuid"];
        return this;
    }

    // Role

    /// <summary>
    /// Получает роль авторизованного пользователя
    /// </summary>
    /// <returns>Роль в виде строки</returns>
    public string GetRole() => Preferences.Default.Role;

    /// <summary>
    /// Устанавливает роль пользователя
    /// </summary>
    /// <param name="role">Роль</param>
    /// <returns>Менеджер пользовательских настроек</returns>
    /// <exception cref="NotSupportedException">Если роль не поддерживается</exception>
    public PreferencesManager SetRole(string role)
    {
        if (!_roles.Contains(role))
            throw new NotSupportedException($"Роль {role} не поддерживается");

        Preferences.Default.Role = role;
        return this;
    }

    /// <summary>
    /// Возвращает роль к заводским настройкам
    /// </summary>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager ClearRole()
    {
        Preferences.Default.Role = _defaultSettings["Role"];
        return this;
    }

    // IsLoggedIn
    
    /// <summary>
    /// Проверяет, зарегистрирован ли пользователь
    /// </summary>
    /// <returns><b>true</b> если да, иначе – <b>false</b></returns>
    public bool IsLoggedIn() => Preferences.Default.IsLoggedIn;

    /// <summary>
    /// Устанавливает, зарегистрирован ли пользователь
    /// </summary>
    /// <param name="isLoggedIn">Зарегистрирован ли пользователь</param>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager SetLoggedIn(bool isLoggedIn)
    {
        Preferences.Default.IsLoggedIn = isLoggedIn;
        return this;
    }

    /// <summary>
    /// Возвращает статус регистрации пользователя к заводским настройкам
    /// </summary>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager ClearLoggedIn()
    {
        Preferences.Default.IsLoggedIn = _defaultSettings["IsLoggedIn"];
        return this;
    }

    // Theme

    /// <summary>
    /// Получает тему из настроек
    /// </summary>
    /// <returns>Тема приложения</returns>
    public string GetTheme() => Preferences.Default.Theme;

    /// <summary>
    /// Устанавливает тему приложения
    /// </summary>
    /// <param name="theme">Тема (по умолчанию используется системная)</param>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager SetTheme(string theme = "System")
    {
        if (!_themes.Contains(theme))
            throw new NotSupportedException("Тема {theme} не поддерживается приложением");

        Preferences.Default.Theme = theme;
        return this;
    }

    /// <summary>
    /// Возвращает тему к заводским настройкам
    /// </summary>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager ClearTheme()
    {
        Preferences.Default.Theme = _defaultSettings["Theme"];
        return this;
    }

    // FontFamily

    /// <summary>
    /// Получает шрифт из настроек
    /// </summary>
    /// <returns>Шрифт</returns>
    public string GetFontFamily() => Preferences.Default.FontFamily;

    /// <summary>
    /// Устанавливает шрифт
    /// </summary>
    /// <param name="familyName">Название шрифта</param>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager SetFontFamily(string familyName)
    {
        Preferences.Default.FontFamily = familyName;
        return this;
    }

    /// <summary>
    /// Возвращает шрифт к заводским настройкам
    /// </summary>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager ClearFontFamily()
    {
        Preferences.Default.FontFamily = _defaultSettings["FontFamily"];
        return this;
    }

    // UpdateRate

    /// <summary>
    /// Получает частоту обновлений
    /// </summary>
    /// <returns>Частота обновлений</returns>
    public TimeSpan GetUpdateRate() => Preferences.Default.UpdateRate;

    /// <summary>
    /// Устанавливает частоту обновлений
    /// </summary>
    /// <param name="updateRate">Частота обновлений</param>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager SetUpdateRate(TimeSpan updateRate)
    {
        Preferences.Default.UpdateRate = updateRate;
        return this;
    }

    /// <summary>
    /// Возвращает частоту обновлений к заводским настройкам
    /// </summary>
    /// <returns>Менеджер пользовательских настроек</returns>
    public PreferencesManager ClearUpdateRate()
    {
        Preferences.Default.UpdateRate = _defaultSettings["UpdateRate"];
        return this;
    }

    #endregion

    #region Changing state

    /// <summary>
    /// Выводит все настройки
    /// </summary>
    [Conditional("DEBUG")]
    public void PrintValues()
    {
        var preferences = Preferences.Default;
        Type settingsType = preferences.GetType();

        foreach (var property in settingsType.GetProperties())
            if (_defaultSettings.ContainsKey(property.Name))
                Debug.WriteLine($"{property.Name} = {property.GetValue(preferences)}");
    }

    /// <summary>
    /// Сохраняет настройки
    /// </summary>
    public void Save() => Preferences.Default.Save();

    /// <summary>
    /// Сбрасывает все настройки
    /// </summary>
    public void Reset()
    {
        Preferences.Default.Reset();
        Save();
    }

    #endregion
}
