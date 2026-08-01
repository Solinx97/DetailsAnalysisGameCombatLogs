using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Services;

internal class FileDialogService : IFileDialogService
{
    public async Task<string[]?> OpenFilesAsync()
    {
        var topLevel = TopLevel.GetTopLevel(
            Application.Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : null);

        if (topLevel == null)
            return null;


        var files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select combat log files",
                AllowMultiple = true,

                FileTypeFilter =
                [
                    new FilePickerFileType("Combat Logs")
                    {
                        Patterns =
                        [
                            "*.txt"
                        ]
                    }
                ]
            });


        return files
            .Select(x => x.Path.LocalPath)
            .ToArray();
    }
}
