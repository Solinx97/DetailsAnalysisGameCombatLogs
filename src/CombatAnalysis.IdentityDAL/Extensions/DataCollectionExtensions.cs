using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Interfaces;
using CombatAnalysis.IdentityDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.IdentityDAL.Extensions;

public static class DataCollectionExtensions
{
    public static void RegisterDependenciesForDAL(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        var connection = configuration.GetConnectionString(connectionName);

        services.AddDbContext<CombatAnalysisIdentityContext>(options =>
        {
            options.UseSqlServer(connection);
        });

        services.AddScoped<IPkeRepository, PkeRepository>();
        services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();

        services.AddScoped<IResetTokenRepository, ResetTokenRepository>();
    }
}
