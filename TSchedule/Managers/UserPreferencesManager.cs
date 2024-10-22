using iNKORE.UI.WPF.Modern;
using System.Runtime.InteropServices;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Managers;

public partial class UserPreferencesManager : IManager
{
    [return: MarshalAs(UnmanagedType.Bool)]
    [LibraryImport("UXTheme.dll", EntryPoint = "#138", SetLastError = true)]
    private static partial bool ShouldSystemUseDarkMode();

    public static bool IsDarkModeEnabled() => ShouldSystemUseDarkMode();

    public static void SetupTheme()
        => ThemeManager.Current.ApplicationTheme = UserPreferencesManager.IsDarkModeEnabled()
            ? ApplicationTheme.Dark : ApplicationTheme.Light;
}
