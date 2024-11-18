using System.Windows;
using System.Windows.Input;
using TSchedule.ViewModels.Pages.MainWindow;

namespace TSchedule.Views.Pages.MainWindow;

public partial class SettingsPage
{
    public SettingsPage() => InitializeComponent();

    private void Page_Loaded(object sender, RoutedEventArgs e)
        => (DataContext as SettingsViewModel)!.ResetDialog = ResetDialog;

    private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        // ReSharper disable once PossibleLossOfFraction
        ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta / 2);
        e.Handled = true;
    }
}