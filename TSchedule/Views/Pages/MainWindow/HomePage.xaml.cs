using iNKORE.UI.WPF.Helpers;
using System.Windows;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.ViewModels;
using TSchedule.ViewModels.Pages.MainWindow;

namespace TSchedule.Views.Pages.MainWindow;

public partial class HomePage
{
    public HomePage() => InitializeComponent();

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (OSVersionHelper.IsWindows10OrGreater && PreferencesManager.Default.IsModernUIEnabled())
            DataContext = new HomeViewModel(WindowManager.Default.GetViewModel<Views.MainWindow>()!
                .As<MainWindowViewModel>()!.NavigationFrame);
        else
            DataContext = new HomeViewModel(WindowManager.Default.GetViewModel<MainWindowWin7>()!
                .As<MainWindowViewModel>()!.NavigationFrame);
    }
}