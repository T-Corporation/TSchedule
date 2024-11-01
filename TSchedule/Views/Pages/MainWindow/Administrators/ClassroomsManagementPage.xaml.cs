using System.Windows;
using TSchedule.ViewModels;

namespace TSchedule.Views.Pages.MainWindow.Administrators;

public partial class ClassroomsManagementPage
{
    public ClassroomsManagementPage() => InitializeComponent();

    private async void Page_Loaded(object sender, RoutedEventArgs e)
        => DataContext = await ClassroomsManagementViewModel.CreateInstanceAsync(AddButton, ClassroomFlyout);
}
