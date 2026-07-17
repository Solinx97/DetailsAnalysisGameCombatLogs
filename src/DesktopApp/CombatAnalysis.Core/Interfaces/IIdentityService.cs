namespace CombatAnalysis.Core.Interfaces;

public interface IIdentityService
{
    Task SendAuthorizationRequestAsync(string authorizationRequestType, CancellationToken cancellationToken);

    Task SendTokenRequestAsync(CancellationToken cancellationToken);
}
