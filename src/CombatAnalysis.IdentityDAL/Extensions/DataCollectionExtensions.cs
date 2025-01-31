using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Interfaces;
using CombatAnalysis.IdentityDAL.Repositories;
using CombatAnalysis.IdentityDAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.IdentityDAL.Extensions;

public static class DataCollectionExtensions
{
    public static void RegisterDependenciesForDAL(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CombatAnalysisIdentityContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IPkeRepository, PkeRepository>();
        services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();

        services.AddScoped<IResetTokenRepository, ResetTokenRepository>();
        services.AddScoped<IVerifyEmailTokenRepository, VerifyEmailTokenRepository>();
    }
}
