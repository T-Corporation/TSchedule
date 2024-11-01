using System.Windows;
using TSchedule.ViewModels.Pages.MainWindow.Teachers;

namespace TSchedule.Views.Pages.MainWindow.Teachers;

public partial class CreateAnnouncementPage
{
    public CreateAnnouncementPage() => InitializeComponent();

    private async void Page_Loaded(object _, RoutedEventArgs __)
        => DataContext = await CreateAnnouncementsViewModel.CreateInstanceAsync(AddButton, AnnouncementFlyout);
}
