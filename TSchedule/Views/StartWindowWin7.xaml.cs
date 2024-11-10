using TSchedule.ViewModels;

namespace TSchedule.Views;

public partial class StartWindowWin7
{
    public StartWindowWin7()
    {
        InitializeComponent();
        DataContext = new StartWindowViewModel(NavigationFrame);
    }
}
