using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Services;
using CombatAnalysis.IdentityDAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterIdentityDependencies(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        services.RegisterDependenciesForDAL(configuration, connectionName);

        services.AddScoped<IOAuthCodeFlowService, OAuthCodeFlowService>();
        services.AddScoped<IIdentityUserService, IdentityUserService>();
        services.AddScoped<IClientService, ClientService>();
    }
}
