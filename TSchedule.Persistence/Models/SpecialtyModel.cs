using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class SpecialtyModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _code = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    public override string ToString() => $"{Code} {Name}";

    public Specialty ToEntity()
        => new()
        {
            Id = Id,
            Code = Code,
            Name = Name
        };

    public override int GetHashCode()
        => Id.GetHashCode();

    public override bool Equals(object? obj)
        => obj is SpecialtyModel sp && sp.GetHashCode() == GetHashCode();
}
