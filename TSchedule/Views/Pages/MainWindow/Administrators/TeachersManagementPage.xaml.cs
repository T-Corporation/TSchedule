using System.Windows;
using TSchedule.ViewModels;

namespace TSchedule.Views.Pages.MainWindow.Administrators;

public partial class TeachersManagementPage
{
    public TeachersManagementPage() => InitializeComponent();

    private async void Page_Loaded(object sender, RoutedEventArgs e)
        => DataContext = await TeachersManagementViewModel.CreateInstanceAsync(AddButton, TeacherFlyout);
}
