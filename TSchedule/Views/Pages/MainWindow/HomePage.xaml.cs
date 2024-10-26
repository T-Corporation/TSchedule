using System.Windows;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.ViewModels;
using TSchedule.ViewModels.Pages.MainWindow;

namespace TSchedule.Views.Pages.MainWindow;

public partial class HomePage
{
    public HomePage() => InitializeComponent();

    private void Page_Loaded(object sender, RoutedEventArgs e)
        => DataContext = new HomePageViewModel(WindowManager.Default.GetViewModel<Views.MainWindow>()!
            .As<MainWindowViewModel>()!.NavigationFrame);
}