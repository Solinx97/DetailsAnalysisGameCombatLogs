using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface IFileDialogService
{
    Task<string[]?> OpenFilesAsync();
}
