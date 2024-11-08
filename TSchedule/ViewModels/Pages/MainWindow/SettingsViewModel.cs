using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TSchedule.Managers;
using TSchedule.Views;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class SettingsViewModel : ObservableObject
{
    private string _correctTheme = PreferencesManager.Default.GetTheme();

    [ObservableProperty]
    private string _theme = PreferencesManager.Default.GetTheme() switch
    {
        "dark" => "Темная",
        "light" => "Светлая",
        _ => "Системная"
    };

    public string[] Themes =>
    [
        "Светлая", "Темная", "Системная"
    ];

    partial void OnThemeChanged(string value)
    {
        _correctTheme = value switch
        {
            "Темная" => "dark",
            "Светлая" => "light",
            _ => "system"
        };

        PreferencesManager.Default.SetTheme(_correctTheme);
    }

    [ObservableProperty]
    private string _fontFamily;

    public ObservableCollection<string> FontFamilies { get; } = [];

    partial void OnFontFamilyChanged(string value)
        => PreferencesManager.Default.SetFontFamily(value);

    public SettingsViewModel()
    {
        foreach (var fontFamily in CustomizationManager.FontFamilies)
            FontFamilies.Add(fontFamily);

        FontFamily = FontFamilies.First(ff => ff == PreferencesManager.Default.GetFontFamily());
    }

    [RelayCommand]
    private void SaveSettings() => UpdateUI();

    [RelayCommand]
    private void ResetSettings()
    {
        PreferencesManager.Default.ClearTheme();
        PreferencesManager.Default.ClearFontFamily();
        PreferencesManager.Default.ClearConnectionString();
        FontFamily = PreferencesManager.Default.GetFontFamily();
        Theme = PreferencesManager.Default.GetTheme() switch
        {
            "dark" => "Темная",
            "light" => "Светлая",
            _ => "Системная"
        };
        UpdateUI();
    }

    [RelayCommand]
    private void SetConnection()
        => WindowManager.Default.CreateWindow<UserConnectionWindow>(showDialog: true);

    private static void UpdateUI()
    {
        PreferencesManager.Default.Save();

        CustomizationManager.Default.LoadStyleFromSettings()
            .ApplyThemeFromPreferences(App.UISettings);
    }
}
