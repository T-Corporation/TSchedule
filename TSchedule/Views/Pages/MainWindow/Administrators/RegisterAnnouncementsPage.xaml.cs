using TSchedule.ViewModels.Pages.MainWindow.Administrators;

namespace TSchedule.Views.Pages.MainWindow.Administrators;

public partial class RegisterAnnouncementsPage
{
    public RegisterAnnouncementsPage() => InitializeComponent();

    private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        => DataContext = await RegisterAnnouncementsViewModel.CreateInstanceAsync();
}