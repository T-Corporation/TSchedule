using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.Views;
using TSchedule.Views.Pages.ImportExportWindow;
using TSchedule.ViewModels.Pages.ImportExportWindow;
using TSchedule.Persistence.Models;
using System.IO;
using TSchedule.Extensions;
using TSchedule.Views.Pages.MainWindow.Administrators;
using TSchedule.ViewModels.Pages.MainWindow.Administrators;

namespace TSchedule.ViewModels;

public partial class ImportExportViewModel : ObservableObject
{
    public Frame NavigationFrame { get; }

    private WizardType WizardType { get; }

    private ExcelManager.LessonSchedules Schedules { get; }

    private WizardResult _result = null!;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GoForwardCommand))]
    [NotifyCanExecuteChangedFor(nameof(FinishCommand))]
    [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
    private bool _isFinished;

    public ImportExportViewModel(Frame frame, WizardType wizardType, ExcelManager.LessonSchedules schedules)
    {
        NavigationFrame = frame;
        WizardType = wizardType;
        Schedules = schedules;
        NavigationFrame.Navigate(new SourcePage(wizardType));
    }

    private bool CanCancel() => IsFinished;

    private bool CannotCancel() => !IsFinished;

    [RelayCommand(CanExecute = nameof(CanCancel))]
    public void GoBack() => NavigationFrame.GoBack();

    [RelayCommand(CanExecute = nameof(CannotCancel))]
    public async Task GoForward()
    {
        if (NavigationFrame.Content is not SourcePage sourcePage
            || sourcePage.DataContext is not SourceViewModel sourceViewModel) return;

        var filePath = sourceViewModel.FilePath.Trim();
        var version = sourceViewModel.SelectedVersion;
        var headers = sourceViewModel.FirstRowContainsHeaders;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            WindowManager.ShowMessageBox(
                "Пожалуйста, заполните поле \"Путь к файлу Excel\"",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (!File.Exists(filePath) && WizardType is WizardType.Import)
        {
            WindowManager.ShowMessageBox(
                "Пожалуйста, укажите верный абсолютный путь до файла Excel",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        try
        {
            ExcelManager excelManager = new(filePath, version, headers);
            var fileName = Path.GetFileName(filePath);

            if (WizardType is WizardType.Export)
            {
                await excelManager.ExportScheduleToFile(Schedules);

                _result = new WizardResult(
                    WizardResultType.Success,
                    $"Экспорт расписания в файл {fileName} прошёл успешно!");
            }
            else
            {
                var schedules = await excelManager.GetSchedulesFromFile();

                // Расписания на числитель, знаменатель и группа получены, далее нужно обновить UI и БД в ScheduleManagementPage
                if (WindowManager.Default.GetViewModel<MainWindow>()!
                    .As<MainWindowViewModel>()!
                    .NavigationFrame.Content is ScheduleManagementPage scheduleManagement
                    && scheduleManagement.DataContext is ScheduleManagementViewModel scheduleViewModel)
                {
                    await scheduleViewModel.UpdateSchedules(schedules.NumeratorDailySchedule, schedules.DenominatorDailySchedule);
                    scheduleViewModel.NumeratorDailySchedule = [.. schedules.NumeratorDailySchedule];
                    scheduleViewModel.DenominatorDailySchedule = [.. schedules.DenominatorDailySchedule];
                }

                _result = new WizardResult(
                    WizardResultType.Success,
                    $"Импорт расписания из файла {fileName} прошёл успешно!");
            }
        }
        catch (Exception ex)
        {
            _result = new WizardResult(WizardResultType.Error, ex.Message);
        }
        
        NavigationFrame.Navigate(new ResultPage(_result));
        IsFinished = true;
    }

    [RelayCommand(CanExecute = nameof(CanCancel))]
    public void Finish()
    {
        WindowManager.Default.GetWindow<ImportExportWindow>()!.Silent = true;
        WindowManager.Default.CloseWindow<ImportExportWindow>();
    }

    [RelayCommand(CanExecute = nameof(CannotCancel))]
    public void Cancel() => WindowManager.Default.CloseWindow<ImportExportWindow>();
}
