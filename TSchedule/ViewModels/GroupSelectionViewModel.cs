using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;
using TSchedule.ViewModels.Pages.MainWindow.Administrators;

namespace TSchedule.ViewModels;

public partial class GroupSelectionViewModel : ObservableObject
{
    private static readonly IGroupsService GroupsService
        = ServiceManager.Default.GetRequiredService<IGroupsService>();

    private ContentDialog _groupSelectionDialog;

    [ObservableProperty]
    private ObservableCollection<GroupModel> _groups = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
    private GroupModel? _selectedGroup;

    [ObservableProperty]
    private byte[] _semesters = [1, 2];

    [ObservableProperty]
    private byte _selectedSemester = ScheduleManagementViewModel.CurrentSemester;

    [ObservableProperty]
    private IEnumerable<short> _years = Enumerable.Range(1991, ScheduleManagementViewModel.CurrentYear - 1990)
        .Select(y => (short)y)
        .ToList()
        .Reverse<short>();

    [ObservableProperty]
    private short _selectedYear = ScheduleManagementViewModel.CurrentYear;

    private GroupSelectionViewModel(ContentDialog dialog, IEnumerable<Group> groups)
    {
        _groupSelectionDialog = dialog;

        foreach (var group in groups)
            Groups.Add(group.ToModel());
    }

    public static async Task<GroupSelectionViewModel> CreateInstanceAsync(ContentDialog dialog)
        => new(dialog, await GroupsService.GetAllGroups());
    
    private bool IsEverythingSelected() => SelectedGroup is not null
        && SelectedYear != 0
        && SelectedSemester != 0;

    [RelayCommand(CanExecute = nameof(IsEverythingSelected))]
    private void Continue()
    {
        _groupSelectionDialog.Hide();
    }
}
