using System.Windows;
using System.Windows.Input;
using TSchedule.ViewModels;

namespace TSchedule.Views.Pages.MainWindow;

public partial class SchedulePage
{
    public SchedulePage()
    {
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        var groupSelectionViewModel = await GroupSelectionViewModel.CreateInstanceAsync(GroupSelectionDialog);
        DataContext = groupSelectionViewModel;
        await GroupSelectionDialog.ShowAsync();
        DataContext = await ScheduleViewModel.CreateInstanceAsync(
            groupSelectionViewModel.SelectedGroup!,
            groupSelectionViewModel.SelectedSemester,
            groupSelectionViewModel.SelectedYear);
    }

    private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not ScheduleViewModel viewModel
            || (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
            || e.Key is not Key.E) return;

        viewModel.OpenExportWizardCommand.Execute(null);
    }
}