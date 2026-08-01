using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface IAsyncInitializable
{
    Task InitializeAsync();
}
