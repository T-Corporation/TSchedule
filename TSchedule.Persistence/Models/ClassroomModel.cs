using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class ClassroomModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _number = string.Empty;

    [ObservableProperty]
    private string _type = string.Empty;

    public Classroom ToEntity()
        => new()
        {
            Id = Id,
            Number = Number,
            Type = Type
        };
}
