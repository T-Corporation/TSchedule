using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using TSchedule.Persistence.Enums;
using TSchedule.Views.Pages.ImportExportWindow;

namespace TSchedule.ViewModels;

public partial class ImportExportViewModel : ObservableObject
{
    private readonly WizardType _wizardType;

    public Frame NavigationFrame { get; }

    public ImportExportViewModel(Frame frame, WizardType wizardType)
    {
        NavigationFrame = frame;
        _wizardType = wizardType;
        NavigateTo(new SourcePage());
    }

    [RelayCommand]
    private void GoBack()
        => NavigationFrame.GoBack();

    [RelayCommand]
    private void GoForward()
        => NavigationFrame.GoForward();

    [RelayCommand]
    private void Finish()
    {

    }

    [RelayCommand]
    private void NavigateTo(object obj)
        => NavigationFrame.Navigate(obj);
}
