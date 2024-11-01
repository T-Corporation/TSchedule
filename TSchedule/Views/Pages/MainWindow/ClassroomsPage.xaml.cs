using System.Windows;
using TSchedule.ViewModels.Pages.MainWindow;

namespace TSchedule.Views.Pages.MainWindow;

public partial class ClassroomsPage
{
    public ClassroomsPage() => InitializeComponent();

    private async void Page_Loaded(object sender, RoutedEventArgs e)
        => DataContext = await ClassroomsViewModel.CreateInstanceAsync();
}