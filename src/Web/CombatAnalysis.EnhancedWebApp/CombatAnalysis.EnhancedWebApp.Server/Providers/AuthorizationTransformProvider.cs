using CombatAnalysis.EnhancedWebApp.Server.Enums;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace CombatAnalysis.EnhancedWebApp.Server.Providers;

public class AuthorizationTransformProvider(IHttpContextAccessor httpContextAccessor) : ITransformProvider
{
    public void ValidateRoute(TransformRouteValidationContext context)
    {
    }

    public void ValidateCluster(TransformClusterValidationContext context)
    {
    }

    public void Apply(TransformBuilderContext context)
    {
        context.AddRequestTransform(transformContext =>
        {
            var httpContext = transformContext.HttpContext;

            if (httpContext is not null && httpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
            {
                transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return ValueTask.CompletedTask;
        });
    }
}
