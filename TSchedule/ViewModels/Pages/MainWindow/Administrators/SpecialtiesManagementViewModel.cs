using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class SpecialtiesManagementViewModel : ObservableObject
{
    public readonly Lazy<ISpecialtiesService> SpecialtiesService
        = new(ServiceManager.Default.GetRequiredService<ISpecialtiesService>);

    public Flyout AttachedFlyout { get; }

    public FrameworkElement Target { get; }

    [ObservableProperty]
    private SpecialtyModel? _specialty;

    [ObservableProperty]
    private ObservableCollection<SpecialtyModel> _specialties = [];

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _specialtyCode = string.Empty;

    [ObservableProperty]
    private string _specialtyName = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public SpecialtiesManagementViewModel(IEnumerable<Specialty> specialties, FrameworkElement element, Flyout flyout)
    {
        Target = element;
        AttachedFlyout = flyout;

        foreach (var specialty in specialties)
            Specialties.Add(specialty.ToModel());
    }

    public static async Task<SpecialtiesManagementViewModel> CreateInstanceAsync(FrameworkElement element, Flyout flyout)
        => new SpecialtiesManagementViewModel(
            await ServiceManager.Default.GetRequiredService<ISpecialtiesService>().GetAllSpecialties(), element, flyout);

    [RelayCommand]
    private void ShowFlyout()
    {
        IsEditing = false;
        ErrorMessage = string.Empty;
        SpecialtyCode = string.Empty;
        SpecialtyName = string.Empty;

        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand]
    private void ShowEditFlyout()
    {
        IsEditing = true;
        ErrorMessage = string.Empty;

        if (Specialty is null)
        {
            WindowManager.ShowMessageBox(
                text: "Не выбрана специальность",
                caption: "Ошибка",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
            return;
        }

        SpecialtyCode = Specialty.Code;
        SpecialtyName = Specialty.Name;
        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand]
    private void HideFlyout() => AttachedFlyout.Hide();

    [RelayCommand]
    private async Task AddOrEdit()
    {
        ErrorMessage = string.Empty;
        const string pleaseFillField = "Пожалуйста, заполните поле \"{0}\"";

        if (string.IsNullOrEmpty(SpecialtyCode))
        {
            ErrorMessage = string.Format(pleaseFillField, "Код");
            return;
        }

        if (string.IsNullOrEmpty(SpecialtyName))
        {
            ErrorMessage = string.Format(pleaseFillField, "Название");
            return;
        }

        Specialty specialty = new()
        {
            Code = SpecialtyCode.Trim(),
            Name = SpecialtyName.Trim()
        };

        if (IsEditing)
        {
            if (Specialty is null)
            {
                WindowManager.ShowMessageBox(
                text: "Не выбрана специальность",
                    caption: "Ошибка",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error);
                return;
            }

            try
            {
                specialty.Id = Specialty.Id;
                await SpecialtiesService.Value.UpdateSpecialty(specialty);
                var foundSpecialty = Specialties.First(s => s.Id == Specialty.Id);
                foundSpecialty.Code = specialty.Code;
                foundSpecialty.Name = specialty.Name;
                HideFlyout();
            }
            catch (UniqueException ue)
            {
                ErrorMessage = ue.Message;
            }
            
            return;
        }

        try
        {
            await SpecialtiesService.Value.AddSpecialty(specialty);
            Specialties.Add(specialty.ToModel());
            HideFlyout();
        }
        catch (UniqueException ue)
        {
            ErrorMessage = ue.Message;
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (Specialty is null)
        {
            WindowManager.ShowMessageBox(
                text: "Не выбрана специальность",
                caption: "Ошибка",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
            return;
        }

        if (WindowManager.ShowMessageBox(
        text: "Вы уверены, что хотите удалить специальность?",
            caption: "Подтверждение",
            button: MessageBoxButton.YesNo,
            icon: MessageBoxImage.Question) is not MessageBoxResult.Yes)
            return;

        await SpecialtiesService.Value.RemoveSpecialty(Specialty.Id);
        Specialties.Remove(Specialties.First(s => s.Id == Specialty.Id));
    }
}
