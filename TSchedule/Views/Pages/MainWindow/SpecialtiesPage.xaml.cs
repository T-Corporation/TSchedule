using System.Windows;
using TSchedule.ViewModels.Pages.MainWindow;

namespace TSchedule.Views.Pages.MainWindow;

public partial class SpecialtiesPage
{
    public SpecialtiesPage() => InitializeComponent();

    private async void Page_Loaded(object sender, RoutedEventArgs e)
        => DataContext = await SpecialtiesViewModel.CreateInstanceAsync();
}
