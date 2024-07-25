namespace CombatAnalysis.Core.Interfaces;

public interface IIdentityService
{
    Task SendAuthorizationRequestAsync(string authorizationRequestType);

    Task SendTokenRequestAsync();
}
