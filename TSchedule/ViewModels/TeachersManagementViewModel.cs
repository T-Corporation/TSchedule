using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Views;

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

        WindowManager.Default.GetViewModel<MainWindow>()!
            .As<MainWindowViewModel>()!
            .NavigationTitle = "Управление преподавателями";
    }

    public static async Task<TeachersManagementViewModel> CreateInstanceAsync()
    {
        var teachers = await ServiceManager.Default.GetRequiredService<ITeachersService>()
            .GetAllTeachers();
        return new TeachersManagementViewModel(teachers);
    }
}
