using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages;

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

    private GroupSelectionViewModel(ContentDialog dialog, IEnumerable<Group> groups)
    {
        _groupSelectionDialog = dialog;

        foreach (var group in groups)
            Groups.Add(group.ToModel());
    }

    public static async Task<GroupSelectionViewModel> CreateInstanceAsync(ContentDialog dialog)
        => new(dialog,
            await GroupsService.GetAllGroups());
    
    private bool IsGroupSelected() => SelectedGroup is not null;

    [RelayCommand(CanExecute = nameof(IsGroupSelected))]
    private void Continue()
    {
        _groupSelectionDialog.Hide();
    }
}
