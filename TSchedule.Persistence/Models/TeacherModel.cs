using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Models;

public partial class TeacherModel : ApplicationUserModel
{
    [ObservableProperty]
    private DateOnly? _dateOfBirth;

    [ObservableProperty]
    private ClassroomModel? _classroom;

    [ObservableProperty]
    private ObservableCollection<TeacherPreferredTimeModel> _preferredTimes = [];

    [ObservableProperty]
    private SubjectModel? _subject;

    public override Role Role => Role.Преподаватель;

    public Teacher ToEntity()
        => new()
        {
            Id = Id,
            DateOfBirth = DateOfBirth,
            ClassroomId = Classroom.Id,
            PreferredTimes = [.. PreferredTimes.Select(tpt => tpt.ToEntity())],
            IsDeleted = IsDeleted,
            UserName = UserName,
            PasswordHash = PasswordHash,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Subject = Subject.ToEntity(),
            FullName = FullName
        };
}
