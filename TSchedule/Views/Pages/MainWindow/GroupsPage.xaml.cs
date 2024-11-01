using System.Windows;
using TSchedule.ViewModels.Pages.MainWindow;

namespace TSchedule.Views.Pages.MainWindow;

public partial class GroupsPage
{
    public GroupsPage() => InitializeComponent();

    private async void Page_Loaded(object sender, RoutedEventArgs e)
        => DataContext = await GroupsViewModel.CreateInstanceAsync();
}