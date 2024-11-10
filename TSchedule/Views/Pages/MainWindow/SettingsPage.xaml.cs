using System.Windows;
using TSchedule.ViewModels.Pages.MainWindow;

namespace TSchedule.Views.Pages.MainWindow;

public partial class SettingsPage
{
    public SettingsPage() => InitializeComponent();

    private void Page_Loaded(object sender, RoutedEventArgs e)
        => (DataContext as SettingsViewModel)!.ResetDialog = ResetDialog;
}