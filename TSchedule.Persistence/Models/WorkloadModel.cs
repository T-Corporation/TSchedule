using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class WorkloadModel : ObservableObject
{
    [ObservableProperty]
    private int _id;
    
    [ObservableProperty]
    private short _hours; // Количество часов (либо в неделю, либо в семестр)
    
    [ObservableProperty]
    private bool _isForSemester; // True для семестра (для студентов), False для недели (для преподавателей)
    
    [ObservableProperty]
    private TeacherModel? _teacher; // Нагрузка на преподавателя
    
    [ObservableProperty]
    private GroupModel? _group; // Нагрузка на студентов

    [ObservableProperty]
    private SubjectModel? _subject;

    public Workload ToEntity()
        => new()
        {
            Id = Id,
            Hours = Hours,
            IsForSemester = IsForSemester,
            TeacherId = Teacher!.Id,
            GroupId = Group!.Id,
            SubjectId = Subject!.Id,
        };
}
