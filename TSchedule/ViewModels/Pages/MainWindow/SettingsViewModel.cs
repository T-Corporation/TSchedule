using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Helpers;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using TSchedule.Managers;
using TSchedule.Persistence.Models;
using TSchedule.Views;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class SettingsViewModel : ObservableObject
{
    #region Properties

    [ObservableProperty]
    private bool _currentModernUIEnabled = PreferencesManager.Default.IsModernUIEnabled();

    public ContentDialog ResetDialog { get; set; } = null!;

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
    private bool _isModernUIEnabled = PreferencesManager.Default.IsModernUIEnabled();

    partial void OnIsModernUIEnabledChanged(bool value) => PreferencesManager.Default.SetModernUIEnabled(value);

    [ObservableProperty]
    private string _fontFamily;

    public ObservableCollection<string> FontFamilies => CustomizationManager.FontFamilies;

    partial void OnFontFamilyChanged(string value) => PreferencesManager.Default.SetFontFamily(value);

    [ObservableProperty]
    private byte _fontSize = PreferencesManager.Default.GetFontSize();

    partial void OnFontSizeChanged(byte value) => PreferencesManager.Default.SetFontSize(value);

    public ObservableCollection<byte> FontSizes => CustomizationManager.FontSizes;

    [ObservableProperty]
    private ObservableCollection<MusicFolder> _musicFolders = MusicManager.Default.LoadMusicFolders();

    [ObservableProperty]
    private Track? _selectedTrack;

    partial void OnSelectedTrackChanged(Track? value) => PlaySelectedTrack(value);

    [ObservableProperty]
    private bool _loop;

    #endregion

    #region Constructor

    public SettingsViewModel()
    {
        FontFamily = FontFamilies.First(ff => ff == PreferencesManager.Default.GetFontFamily());
    }

    #endregion

    #region Commands

    [RelayCommand]
    private async Task SaveSettings()
    {
        if (CurrentModernUIEnabled != IsModernUIEnabled)
            await ResetDialog.ShowAsync();
        CurrentModernUIEnabled = IsModernUIEnabled;
        UpdateUI();
    }

    [RelayCommand]
    private void ResetSettings()
    {
        PreferencesManager.Default.ClearTheme();
        PreferencesManager.Default.ClearModernUIEnabled();
        PreferencesManager.Default.ClearFontFamily();
        PreferencesManager.Default.ClearFontSize();
        PreferencesManager.Default.ClearConnectionString();
        IsModernUIEnabled = PreferencesManager.Default.IsModernUIEnabled();
        FontFamily = PreferencesManager.Default.GetFontFamily();
        FontSize = PreferencesManager.Default.GetFontSize();
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
    {
        if (OSVersionHelper.IsWindows10OrGreater && PreferencesManager.Default.IsModernUIEnabled())
            WindowManager.Default.CreateWindow<UserConnectionWindow>(showDialog: true);
        else
            WindowManager.Default.CreateWindow<UserConnectionWindowWin7>(showDialog: true);
    }

    [RelayCommand]
    private void AlterModernUIEnabled() => PreferencesManager.Default.SetModernUIEnabled(CurrentModernUIEnabled);

    [RelayCommand]
    private void RebootApplication()
    {
        PreferencesManager.Default.Save();
        App.Restart();
    }

    [RelayCommand]
    private void PlaySelectedTrack(Track? track)
        => MusicManager.Default.PlayTrack(track ?? SelectedTrack, Loop);

    [RelayCommand]
    private void Stop() => MusicManager.Default.Stop();

    #endregion

    #region Methods

    private static void UpdateUI()
    {
        PreferencesManager.Default.Save();

        CustomizationManager.Default.LoadStyleFromSettings()
            .ApplyThemeFromPreferences(App.UISettings);
    }

    #endregion
}
