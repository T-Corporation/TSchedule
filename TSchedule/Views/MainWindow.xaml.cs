using System.Windows.Navigation;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.ViewModels;

namespace TSchedule.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(ContentFrame);
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        var viewModel = WindowManager.Default.GetViewModel<MainWindow>()!
            .As<MainWindowViewModel>()!;

        // Определяем, какой элемент должен быть выделен на основе навигации
        var pageCode = e.Content.ToPageCode();

        viewModel.NavigationItem = viewModel.NavigationItems
            .FirstOrDefault(item => item.Tag is PageCode pc
                && pc == pageCode
                && pageCode is not PageCode.Profile and not PageCode.Settings);
    }
}
