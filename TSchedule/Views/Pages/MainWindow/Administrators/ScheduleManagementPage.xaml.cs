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
        DataContext = await ScheduleManagementViewModel.CreateInstanceAsync(
            EditFlyout,
            groupSelectionViewModel.SelectedGroup!,
            groupSelectionViewModel.SelectedSemester,
            groupSelectionViewModel.SelectedYear);
    }

    private async void Page_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not ScheduleManagementViewModel viewModel)
            return;

        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            switch (e.Key)
            {
                case Key.I:
                    viewModel.OpenWizardCommand.Execute("Import");
                    break;

                case Key.E:
                    viewModel.OpenWizardCommand.Execute("Export");
                    break;
            }
            return;
        }

        if (e.OriginalSource is not DataGridCell cell
            || cell.Column is null
            || cell.DataContext is not LessonScheduleModel selectedItem)
            return;

        var dataGrid = DenominatorTab.IsSelected ? DenominatorGrid : NumeratorGrid;
        SetSelectedSchedule(viewModel, selectedItem, cell.Column.DisplayIndex, dataGrid.Items.IndexOf(selectedItem));
        viewModel.IsDenominator = DenominatorTab.IsSelected;

        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            switch (e.Key)
            {
                case Key.C:
                    CopyCell(selectedItem, cell.Column.DisplayIndex);
                    break;

                case Key.V:
                    await PasteCell(selectedItem, cell.Column.DisplayIndex);
                    break;
            }
            return;
        }

        switch (e.Key)
        {
            case Key.Insert:
                EditFlyout.ShowAt(TabControl);
                break;

            case Key.Delete:
                await TryDeleteSchedule(viewModel);
                break;
        }
    }

    private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedItem(sender, out var viewModel, out var selectedItem, out var columnIndex, out var rowIndex))
            return;

        SetSelectedSchedule(viewModel, selectedItem, columnIndex, rowIndex);
        viewModel.IsDenominator = DenominatorTab.IsSelected;

        CopyCell(selectedItem, columnIndex);
    }

    private async void PasteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedItem(sender, out var viewModel, out var selectedItem, out var columnIndex, out var rowIndex))
            return;

        SetSelectedSchedule(viewModel, selectedItem, columnIndex, rowIndex);
        viewModel.IsDenominator = DenominatorTab.IsSelected;

        await PasteCell(selectedItem, columnIndex);
    }

    private void EditMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedItem(sender, out var viewModel, out var selectedItem, out var columnIndex, out var rowIndex))
            return;

        SetSelectedSchedule(viewModel, selectedItem, columnIndex, rowIndex);
        viewModel.IsDenominator = DenominatorTab.IsSelected;

        EditFlyout.ShowAt(TabControl);
    }

    private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedItem(sender, out var viewModel, out var selectedItem, out var columnIndex, out var rowIndex))
            return;

        SetSelectedSchedule(viewModel, selectedItem, columnIndex, rowIndex);
        viewModel.IsDenominator = DenominatorTab.IsSelected;

        await TryDeleteSchedule(viewModel);
    }

    private static void SetSelectedSchedule(
        ScheduleManagementViewModel viewModel,
        LessonScheduleModel selectedItem,
        int columnIndex,
        int rowIndex)
    {
        try
        {
            viewModel.SelectedSchedule = columnIndex switch
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

            viewModel.SelectedDayOfWeek = columnIndex switch
            {
                1 => WeekDays.Monday,
                2 => WeekDays.Tuesday,
                3 => WeekDays.Wednesday,
                4 => WeekDays.Thursday,
                5 => WeekDays.Friday,
                6 => WeekDays.Saturday,
                7 => WeekDays.Sunday,
                _ => throw new NotSupportedException("Неверный индекс столбца для дня недели")
            };

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
        }
        catch (NotSupportedException)
        {
            // ignored
        }
    }

    private bool TryGetSelectedItem(
        object sender,
        out ScheduleManagementViewModel viewModel,
        out LessonScheduleModel selectedItem,
        out int columnIndex,
        out int rowIndex)
    {
        viewModel = null!;
        selectedItem = null!;
        columnIndex = -1;
        rowIndex = -1;

        if (sender is not MenuItem menuItem
            || menuItem.Parent is not ContextMenu contextMenu
            || contextMenu.PlacementTarget is not DataGrid dataGrid
            || dataGrid.SelectedCells[0].Column is not DataGridTextColumn column
            || dataGrid.SelectedCells[0].Item is not LessonScheduleModel selectedItemTemp
            || DataContext is not ScheduleManagementViewModel viewModelTemp)
            return false;

        viewModel = viewModelTemp;
        selectedItem = selectedItemTemp;
        columnIndex = column.DisplayIndex;
        rowIndex = dataGrid.Items.IndexOf(selectedItem);

        return true;
    }

    private static async Task TryDeleteSchedule(ScheduleManagementViewModel viewModel)
    {
        if (viewModel.SelectedSchedule is null)
        {
            ShowWarningMessage("Не выбран предмет для удаления");
            return;
        }

        if (viewModel.SelectedSchedule.Id == 0)
        {
            ShowWarningMessage("Запись ещё не сохранена в БД. Пожалуйста, перезагрузите страницу");
            return;
        }

        await viewModel.DeleteCommand.ExecuteAsync(null);
    }

    private static void ShowWarningMessage(string text, string caption = "Предупреждение")
    {
        WindowManager.ShowMessageBox(
            text: text,
            caption: caption,
            button: MessageBoxButton.OK,
            icon: MessageBoxImage.Warning);
    }

    private static void CopyCell(LessonScheduleModel selectedItem, int columnIndex)
        => Clipboard.SetText(columnIndex switch
        {
            1 => selectedItem.Monday!.Teacher!.Subject!.Name,
            2 => selectedItem.Tuesday!.Teacher!.Subject!.Name,
            3 => selectedItem.Wednesday!.Teacher!.Subject!.Name,
            4 => selectedItem.Thursday!.Teacher!.Subject!.Name,
            5 => selectedItem.Friday!.Teacher!.Subject!.Name,
            6 => selectedItem.Saturday!.Teacher!.Subject!.Name,
            7 => selectedItem.Sunday!.Teacher!.Subject!.Name,
            _ => throw new NotSupportedException("Неверный индекс столбца для дня недели"),
        });

    private async Task PasteCell(LessonScheduleModel selectedItem, int columnIndex)
    {
        if (DataContext is not ScheduleManagementViewModel viewModel) return;

        // Чтение строки из буфера обмена
        if (Clipboard.GetDataObject() is not DataObject dataObject || !dataObject.GetDataPresent(DataFormats.Text))
        {
            WindowManager.ShowMessageBox(
                "Буфер обмена пуст или содержит неверные данные.",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }
        
        string subjectName = Clipboard.GetText().Trim();
        var subject = (await ScheduleManagementViewModel.SubjectsService.GetSubjectByName(subjectName))?.ToModel();

        if (subject is null)
        {
            WindowManager.ShowMessageBox(
                $"Предмет с названием \"{subjectName}\" не найден.",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        viewModel.SelectedSubject = subject;
        
        // Здесь вам нужно установить значение предмета, используя subjectCode
        switch (columnIndex)
        {
            case 1:
                viewModel.SelectedSchedule = selectedItem.Monday;
                break;
            case 2:
                viewModel.SelectedSchedule = selectedItem.Tuesday;
                break;
            case 3:
                viewModel.SelectedSchedule = selectedItem.Wednesday;
                break;
            case 4:
                viewModel.SelectedSchedule = selectedItem.Thursday;
                break;
            case 5:
                viewModel.SelectedSchedule = selectedItem.Friday;
                break;
            case 6:
                viewModel.SelectedSchedule = selectedItem.Saturday;
                break;
            case 7:
                viewModel.SelectedSchedule = selectedItem.Sunday;
                break;
        }

        viewModel.SelectedTeacher = (await ScheduleManagementViewModel.TeachersService
            .GetTeacherBySubjectId(viewModel.SelectedSubject.Id))?
            .ToModel();

        viewModel.SelectedClassroom = viewModel.SelectedTeacher?.Classroom;

        await viewModel.SaveCommand.ExecuteAsync(null);

        if (!string.IsNullOrEmpty(viewModel.ErrorMessage))
        {
            WindowManager.ShowMessageBox(
                viewModel.ErrorMessage,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            viewModel.ErrorMessage = string.Empty;
        }
    }
}
