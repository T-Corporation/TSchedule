using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class GroupsViewModel : ObservableObject
{
    private static readonly IGroupsService _classroomsService
        = ServiceManager.Default.GetRequiredService<IGroupsService>();

    [ObservableProperty]
    private ObservableCollection<Group> _groups = [];

    private GroupsViewModel(IEnumerable<Group> groups)
    {
        foreach (var group in groups)
            Groups.Add(group);
    }

    public static async Task<GroupsViewModel> CreateInstanceAsync()
        => new(await _classroomsService.GetAllGroups());
}
