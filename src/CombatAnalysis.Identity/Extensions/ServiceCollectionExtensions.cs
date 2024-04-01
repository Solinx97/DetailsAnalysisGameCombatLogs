using CombatAnalysis.CustomerDAL.Interfaces;
using CombatAnalysis.CustomerDAL.Repositories.SQL;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterIdentityDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAppSecret, SQLSecretRepository>();
        services.AddScoped<IJWTSecret, JWTSecretService>();
        services.AddScoped<IIdentityTokenService, TokenService>();
    }
}
