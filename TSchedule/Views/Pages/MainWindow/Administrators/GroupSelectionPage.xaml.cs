using System.Windows;
using TSchedule.ViewModels.Pages.MainWindow.Administrators;

namespace TSchedule.Views.Pages.MainWindow.Administrators;

public partial class GroupSelectionPage
{
    public GroupSelectionPage() => InitializeComponent();

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        DataContext = await GroupSelectionViewModel.CreateInstanceAsync();
        await GroupSelectionDialog.ShowAsync();
    }
}
