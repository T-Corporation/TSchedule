using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class ClassroomsViewModel : ObservableObject
{
    private static readonly IClassroomsService _classroomsService
        = ServiceManager.Default.GetRequiredService<IClassroomsService>();

    [ObservableProperty]
    private ObservableCollection<Classroom> _classrooms = [];

    private ClassroomsViewModel(IEnumerable<Classroom> classrooms)
    {
        foreach (var classroom in classrooms)
            Classrooms.Add(classroom);
    }

    public static async Task<ClassroomsViewModel> CreateInstanceAsync()
        => new(await _classroomsService.GetAllClassrooms());
}
