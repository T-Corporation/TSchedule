using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class AnnouncementModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private DateTime _createdAt = DateTime.Now; // Дата создания уведомления;

    [ObservableProperty]
    private DateTime _updatedAt = DateTime.Now; // Дата создания уведомления

    [ObservableProperty]
    private DateTime _absentFrom; // Дата и время начала отсутствия

    [ObservableProperty]
    private DateTime _absentTo; // Дата и время окончания отсутствия

    [ObservableProperty]
    private TeacherModel? _teacher;

    [ObservableProperty]
    private string _reason = string.Empty; // Причина отсутствия

    [ObservableProperty]
    private bool _isRegistered;

    public Announcement ToEntity()
        => new()
        {
            Id = Id,
            UpdatedAt = UpdatedAt,
            CreatedAt = CreatedAt,
            AbsentTo = AbsentTo,
            AbsentFrom = AbsentFrom,
            TeacherId = Teacher!.Id,
            Reason = Reason
        };

    public override string ToString() => Reason;
}
