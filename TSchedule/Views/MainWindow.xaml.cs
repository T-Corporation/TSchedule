using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.ViewModels;

namespace TSchedule.Views;

public partial class MainWindow
{
    public bool Silent { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(ContentFrame);
        MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel)
            return;

        // Определяем, какой элемент должен быть выделен на основе навигации
        var pageCode = e.Content.ToPageCode();

        viewModel.NavigationItem = viewModel.NavigationItems
            .FirstOrDefault(item => item.Tag is PageCode pc
                && pc == pageCode
                && pageCode is not PageCode.Settings);
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (Silent) return;

        e.Cancel = WindowManager.ShowMessageBox(
            "Вы уверены, что желаете завершить работу?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question) is not MessageBoxResult.Yes;
    }
}
