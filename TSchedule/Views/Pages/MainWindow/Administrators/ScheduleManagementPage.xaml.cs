using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TSchedule.Managers;
using TSchedule.Persistence.Extensions;
using TSchedule.Persistence.Models;
using TSchedule.ViewModels.Pages;
using TSchedule.ViewModels.Pages.MainWindow.Administrators;

namespace TSchedule.Views.Pages.MainWindow.Administrators;

public partial class ScheduleManagementPage
{
    public ScheduleManagementPage() => InitializeComponent();

    private async void Page_Loaded(object _, RoutedEventArgs __)
    {
        var groupSelectionViewModel = await GroupSelectionViewModel.CreateInstanceAsync(GroupSelectionDialog);
        DataContext = groupSelectionViewModel;
        await GroupSelectionDialog.ShowAsync();
        DataContext = await ScheduleManagementViewModel.CreateInstanceAsync(groupSelectionViewModel.SelectedGroup!);
    }

    private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Insert:
                try
                {
                    if (sender is not DataGrid dataGrid
                        || e.OriginalSource is not DataGridCell cell
                        || cell.Column is null)
                        return;

                    if (cell.DataContext is not LessonScheduleModel selectedItem
                        || DataContext is not ScheduleManagementViewModel viewModel)
                        return;

                    var columnIndex = cell.Column.DisplayIndex;
                    var rowIndex = dataGrid.Items.IndexOf(selectedItem);

                    switch (columnIndex)
                    {
                        case 1:
                            viewModel.SelectedSchedule = selectedItem.Monday;
                            viewModel.SelectedDayOfWeek = WeekDays.Monday;
                            break;

                        case 2:
                            viewModel.SelectedSchedule = selectedItem.Tuesday;
                            viewModel.SelectedDayOfWeek = WeekDays.Tuesday;
                            break;

                        case 3:
                            viewModel.SelectedSchedule = selectedItem.Wednesday;
                            viewModel.SelectedDayOfWeek = WeekDays.Wednesday;
                            break;

                        case 4:
                            viewModel.SelectedSchedule = selectedItem.Thursday;
                            viewModel.SelectedDayOfWeek = WeekDays.Thursday;
                            break;

                        case 5:
                            viewModel.SelectedSchedule = selectedItem.Friday;
                            viewModel.SelectedDayOfWeek = WeekDays.Friday;
                            break;

                        case 6:
                            viewModel.SelectedSchedule = selectedItem.Saturday;
                            viewModel.SelectedDayOfWeek = WeekDays.Saturday;
                            break;

                        case 7:
                            viewModel.SelectedSchedule = selectedItem.Sunday;
                            viewModel.SelectedDayOfWeek = WeekDays.Sunday;
                            break;

                        default:
                            throw new NotSupportedException("Неверный индекс столбца для дня недели");
                    }

                    viewModel.SelectedLesson = rowIndex switch
                    {
                        0 => Lessons.First,
                        1 => Lessons.Second,
                        2 => Lessons.Third,
                        3 => Lessons.Fourth,
                        4 => Lessons.Fifth,
                        5 => Lessons.Sixth,
                        _ => throw new NotSupportedException("Неверный индекс строки для занятия")
                    };

                    EditFlyout.ShowAt(TabControl);
                }
                catch (NotSupportedException nse)
                {
                    Debug.WriteLine("StackTrace:");
                    Debug.WriteLine(nse);
                }
                break;

            case Key.Delete:
                if (WindowManager.ShowMessageBox(
                    text: "Вы уверены, что хотите удалить это занятие?",
                    caption: "Подтверждение",
                    button: MessageBoxButton.YesNo,
                    icon: MessageBoxImage.Question) is not MessageBoxResult.Yes) return;


                break;

            default:
                break;
        }
    }
}
