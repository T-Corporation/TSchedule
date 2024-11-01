using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class SubjectsViewModel : ObservableObject
{
    private static readonly ISubjectsService _subjectsService
        = ServiceManager.Default.GetRequiredService<ISubjectsService>();

    [ObservableProperty]
    private ObservableCollection<Subject> _subjects = [];

    private SubjectsViewModel(IEnumerable<Subject> subjects)
    {
        foreach (var subject in subjects)
            Subjects.Add(subject);
    }

    public static async Task<SubjectsViewModel> CreateInstanceAsync()
        => new(await _subjectsService.GetAllSubjects());
}
