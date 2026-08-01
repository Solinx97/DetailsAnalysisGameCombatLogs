using CombatAnalysis.UploadingLogsApp.Models.Identity;
using CombatAnalysis.UploadingLogsApp.Models.User;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces.Security;

public interface ISecurityStorage
{
    void SaveAccessToken(TokenResponseModel token);

    void RemoveAccessToken();

    Task<AppUserModel?> GetUserAsync(CancellationToken cancellationToken);

    void GetAccessToken();
}
