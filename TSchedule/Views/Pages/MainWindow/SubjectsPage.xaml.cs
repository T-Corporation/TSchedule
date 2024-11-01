using System.Windows;
using TSchedule.ViewModels.Pages.MainWindow;

namespace TSchedule.Views.Pages.MainWindow;

public partial class SubjectsPage
{
    public SubjectsPage() => InitializeComponent();

    private async void Page_Loaded(object sender, RoutedEventArgs e)
        => DataContext = await SubjectsViewModel.CreateInstanceAsync();
}