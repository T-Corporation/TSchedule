using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using TSchedule.Persistence.Interfaces.Bases;
using iNKORE.UI.WPF.Modern;
using Windows.UI.ViewManagement;

namespace TSchedule.Managers;

public class CustomizationManager : IManager
{
    public static readonly IEnumerable<string> FontFamilies = Fonts.SystemFontFamilies.Select(ff => ff.Source);

    /// <summary>
    /// "Ленивое" создание менеджера кастомизации
    /// </summary>
    private static readonly Lazy<CustomizationManager> _instance = new(() => new CustomizationManager());

    /// <summary>
    /// Ссылка на менеджер кастомизации
    /// </summary>
    public static CustomizationManager Default => _instance.Value;

    /// <summary>
    /// Создаёт стили на основе настроек
    /// </summary>
    /// <returns>Менеджер кастомизации</returns>
    public CustomizationManager LoadStyleFromSettings()
    {
        var fontFamilyName = PreferencesManager.Default.GetFontFamily();
        var fontFamily = new FontFamily(fontFamilyName);

        /*// На всякий случай
        var controlStyle = new Style(typeof(Control));
        controlStyle.Setters.Add(new Setter(Control.FontFamilyProperty, fontFamily));
        if (Application.Current.Resources.Contains(typeof(Control)))
            Application.Current.Resources.Remove(typeof(Control));
        Application.Current.Resources.Add(typeof(Control), controlStyle);*/

        var textBlockStyle = new Style(typeof(TextBlock));
        textBlockStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
        if (Application.Current.Resources.Contains(typeof(TextBlock)))
            Application.Current.Resources.Remove(typeof(TextBlock));
        Application.Current.Resources.Add(typeof(TextBlock), textBlockStyle);
        return this;
    }

    /// <summary>
    /// Устанавливает тему, которая указана в настройках
    /// </summary>
    /// <returns>Менеджер кастомизации</returns>
    public CustomizationManager ApplyThemeFromPreferences(UISettings uiSettings)
    {
        string theme = PreferencesManager.Default.GetTheme();
        ApplySystemAccent(uiSettings);

        if (theme is "system")
            ApplySystemTheme();
        else if (theme is "dark")
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
        else
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
        
        return this;
    }

    public CustomizationManager ApplySystemTheme()
    {
        ThemeManager.Current.ApplicationTheme = PreferencesManager.Default.IsDarkModeEnabled
            ? ApplicationTheme.Dark
            : ApplicationTheme.Light;

        return this;
    }

    public CustomizationManager ApplySystemAccent(UISettings uiSettings)
    {
        #pragma warning disable CA1416 // Проверка совместимости платформы
        var accent = uiSettings.GetColorValue(UIColorType.Accent);
        #pragma warning restore CA1416 // Проверка совместимости платформы
        var accentColor = Color.FromArgb(accent.A, accent.R, accent.G, accent.B);
        ThemeManager.Current.AccentColor = accentColor;
        return this;
    }
}
