using TSchedule.ViewModels;

namespace TSchedule.Views;

public partial class StartWindow
{
    public StartWindow()
    {
        InitializeComponent();
        DataContext = new StartWindowViewModel(NavigationFrame);
    }
}