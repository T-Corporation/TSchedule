using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using TSchedule.Persistence.Interfaces.Bases;
using iNKORE.UI.WPF.Modern;

namespace TSchedule.Managers;

public class CustomizationManager : IManager
{
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

        // На всякий случай
        /*var controlStyle = new Style(typeof(Control));
        controlStyle.Setters.Add(new Setter(Control.FontFamilyProperty, fontFamily));
        Current.Resources.Add(typeof(Control), controlStyle);*/

        var textBlockStyle = new Style(typeof(TextBlock));
        textBlockStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
        Application.Current.Resources.Add(typeof(TextBlock), textBlockStyle);
        return this;
    }

    /// <summary>
    /// Устанавливает тему, которая указана в настройках
    /// </summary>
    /// <returns>Менеджер кастомизации</returns>
    public CustomizationManager ApplyCurrentTheme()
    {
        ThemeManager.Current.ApplicationTheme = PreferencesManager.Default.GetTheme() switch
        {
            "Dark" => ApplicationTheme.Dark,
            "Light" => ApplicationTheme.Light,
            _ => PreferencesManager.Default.IsDarkModeEnabled ? ApplicationTheme.Dark : ApplicationTheme.Light
        };
        return this;
    }
}
