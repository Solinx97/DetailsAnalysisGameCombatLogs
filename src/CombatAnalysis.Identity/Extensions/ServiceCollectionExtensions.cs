using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterIdentityDependencies(this IServiceCollection services)
    {
        services.AddScoped<IIdentityTokenService, TokenService>();
    }
}
