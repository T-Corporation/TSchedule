using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;
using TSchedule.Views.Pages.MainWindow.Administrators;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class GroupSelectionViewModel : ObservableObject
{
    private static readonly IGroupsService GroupsService
        = ServiceManager.Default.GetRequiredService<IGroupsService>();

    [ObservableProperty]
    private ObservableCollection<GroupModel> _groups = [];

    [ObservableProperty]
    private GroupModel? _selectedGroup;

    private GroupSelectionViewModel(IEnumerable<Group> groups)
    {
        foreach (var group in groups)
            Groups.Add(group.ToModel());
    }

    public static async Task<GroupSelectionViewModel> CreateInstanceAsync()
        => new(await GroupsService.GetAllGroups());

    private bool IsGroupSelected() => SelectedGroup is not null;

    [RelayCommand(CanExecute = nameof(IsGroupSelected))]
    private void CloseDialog() => WindowManager.Default.GetViewModel<Views.MainWindow>()!
        .As<MainWindowViewModel>()!
        .NavigateTo(new ScheduleManagementPage(SelectedGroup!));

    [RelayCommand]
    private void GoBack() => WindowManager.Default.GetViewModel<Views.MainWindow>()!
        .As<MainWindowViewModel>()!
        .GoBack();
}
