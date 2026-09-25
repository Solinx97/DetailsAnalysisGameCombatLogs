using CombatAnalysis.EnhancedWebApp.Server.Enums;
using System.Net.Http.Headers;

namespace CombatAnalysis.EnhancedWebApp.Server.Handlers;

public class WoWGameDataAuthorizationHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is not null && httpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.BattleNetAccessToken), out var accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
