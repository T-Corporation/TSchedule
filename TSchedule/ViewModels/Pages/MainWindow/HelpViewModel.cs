using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class HelpViewModel : ObservableObject
{
    [RelayCommand]
    private void Download(string ext)
    {
        SaveFileDialog dialog = new()
        {
            FileName = $"Расписание2024_1_ИС-2.{ext}",
            Filter = ExcelManager.ExcelVersionToFileFilter(ext switch
            {
                "xlsx" => ExcelVersion.Excel2007,
                 _ => ExcelVersion.Excel97
            })
        };

        if (dialog.ShowDialog() is not true) return;
        File.Copy($"./Templates/Расписание2024_1_ИС-2.{ext}", dialog.FileName, true);
    }
}
