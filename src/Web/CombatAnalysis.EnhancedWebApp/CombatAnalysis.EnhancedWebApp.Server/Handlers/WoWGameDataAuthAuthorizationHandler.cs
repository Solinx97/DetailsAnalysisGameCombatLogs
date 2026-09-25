using CombatAnalysis.EnhancedWebApp.Server.Consts;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace CombatAnalysis.EnhancedWebApp.Server.Handlers;

public class WoWGameDataAuthAuthorizationHandler(IOptions<BattleNet> battleNet) : DelegatingHandler
{
    private readonly BattleNet _battleNet = battleNet.Value;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_battleNet.ClientId}:{_battleNet.clientSecret}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        return await base.SendAsync(request, cancellationToken);
    }
}
