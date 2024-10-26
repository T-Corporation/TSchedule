using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;

namespace TSchedule.ViewModels;

public partial class TeachersManagementViewModel : ObservableObject
{
    public readonly Lazy<ITeachersService> TeachersService
        = new(ServiceManager.Default.GetRequiredService<ITeachersService>);

    public ObservableCollection<Teacher> Teachers { get; set; } = [];

    [ObservableProperty]
    private Teacher _teacher = null!;

    public TeachersManagementViewModel(IEnumerable<Teacher> teachers)
    {
        foreach (var teacher in teachers)
            Teachers.Add(teacher);
    }

    public static async Task<TeachersManagementViewModel> CreateInstanceAsync()
    {
        return new TeachersManagementViewModel(await ServiceManager.Default.GetRequiredService<ITeachersService>()
            .GetAllTeachers());
    }
}
