using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface IIdentityService
{
    Task SendAuthorizationRequestAsync(string authorizationRequestType, CancellationToken cancellationToken);

    Task SendTokenRequestAsync(CancellationToken cancellationToken);
}
