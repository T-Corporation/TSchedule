using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class SpecialtiesViewModel : ObservableObject
{
    private static readonly ISpecialtiesService _specialtiesService
        = ServiceManager.Default.GetRequiredService<ISpecialtiesService>();

    [ObservableProperty]
    private ObservableCollection<Specialty> _specialties = [];

    private SpecialtiesViewModel(IEnumerable<Specialty> specialties)
    {
        foreach (var specialty in specialties)
            Specialties.Add(specialty);
    }

    public static async Task<SpecialtiesViewModel> CreateInstanceAsync()
        => new(await _specialtiesService.GetAllSpecialties());
}
