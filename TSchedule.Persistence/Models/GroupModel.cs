using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class GroupModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _code = string.Empty;

    [ObservableProperty] // Например, курс может быть от 1 до 5
    private byte _course;

    [ObservableProperty]
    private SpecialtyModel? _specialty;

    [ObservableProperty]
    private ObservableCollection<SubjectModel> _subjects = [];

    public override string ToString() => Code;

    public Group ToEntity()
        => new()
        {
            Id = Id,
            Code = Code,
            Course = Course,
            SpecialtyId = Specialty!.Id
        };
}
