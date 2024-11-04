using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class SubjectModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _code = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private byte _weeklyHours;

    [ObservableProperty]
    private SpecialtyModel? _specialty;

    [ObservableProperty]
    private SubjectModel? _selectedSubject;

    public Subject ToEntity()
        => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            WeeklyHours = WeeklyHours,
            SpecialtyId = Specialty?.Id ?? 0
        };

    public override int GetHashCode()
        => Code.GetHashCode();

    public override bool Equals(object? obj)
        => obj is SubjectModel sm && sm.GetHashCode() == GetHashCode();

    public override string ToString() => Name;
}
