using iNKORE.UI.WPF.Modern.Controls.Primitives;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TSchedule.Managers;
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

                    int index = cell.Column.DisplayIndex;
                    viewModel.SelectedSchedule = index switch
                    {
                        1 => selectedItem.Monday,
                        2 => selectedItem.Tuesday,
                        3 => selectedItem.Wednesday,
                        4 => selectedItem.Thursday,
                        5 => selectedItem.Friday,
                        6 => selectedItem.Saturday,
                        7 => selectedItem.Sunday,
                        _ => throw new NotSupportedException("Неверный индекс столбца для дня недели")
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
