using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class TeachersViewModel : ObservableObject
{
    private static readonly ITeachersService _teachersService
        = ServiceManager.Default.GetRequiredService<ITeachersService>();

    [ObservableProperty]
    private ObservableCollection<Teacher> _teachers = [];

    private TeachersViewModel(IEnumerable<Teacher> teachers)
    {
        foreach (var specialty in teachers)
            Teachers.Add(specialty);
    }

    public static async Task<TeachersViewModel> CreateInstanceAsync()
        => new(await _teachersService.GetAllTeachers());
}
