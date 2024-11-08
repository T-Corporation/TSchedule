using System.ComponentModel;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.ViewModels;

namespace TSchedule.Views;

public partial class ImportExportWindow
{
    public bool Silent { get; set; }
    private readonly WizardType _wizardType;
    private readonly ExcelManager.LessonSchedules _schedules;

    public ImportExportWindow(WizardType wizardType, ExcelManager.LessonSchedules schedules)
    {
        InitializeComponent();
        _wizardType = wizardType;
        _schedules = schedules;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
        => DataContext = new ImportExportViewModel(NavigationFrame, _wizardType, _schedules);

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (Silent) return;

        e.Cancel = WindowManager.ShowMessageBox(
            "Вы уверены, что хотите прервать работу мастера?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question) is not MessageBoxResult.Yes;
    }
}
