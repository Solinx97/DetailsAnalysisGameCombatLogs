using CombatAnalysis.DAL.Consts;
using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories.Firebase;
using CombatAnalysis.DAL.Repositories.SQL;
using CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;
using CombatAnalysis.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.DAL.Extensions;

public static class DataCollectionExtensions
{
    public static void CombatParserDALDependencies(this IServiceCollection services, IConfiguration configuration, string connectionName, int commandTimeout)
    {
        DBConfigurations.CommandTimeout = commandTimeout;

        var databaseName = configuration.GetSection("Database:Name").Value ?? string.Empty;
        switch (databaseName)
        {
            case nameof(DatabaseType.MSSQL):
                MSSQLDatabase(services, configuration, connectionName);
                break;
            case nameof(DatabaseType.Firebase):
                FirebaseDatabase(services);
                break;
            default:
                MSSQLDatabase(services, configuration, connectionName);
                break;
        }
    }

    private static void MSSQLDatabase(IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        var connection = configuration.GetConnectionString(connectionName);

        services.AddDbContext<CombatParserSQLContext>(options =>
        {
            options.UseSqlServer(connection);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IPlayerInfo<CombatPlayerPosition, int>, SQLPlayerInfoRepository<CombatPlayerPosition, int>>();
        services.AddScoped<IPlayerInfo<Combat, int>, SQLPlayerInfoRepository<Combat, int>>();
        services.AddScoped<IPlayerInfoCount<DamageDone, int>, SQLPlayerInfoCountRepository<DamageDone, int>>();
        services.AddScoped<IPlayerInfo<DamageDoneGeneral, int>, SQLPlayerInfoRepository<DamageDoneGeneral, int>>();
        services.AddScoped<IPlayerInfoCount<HealDone, int>, SQLPlayerInfoCountRepository<HealDone, int>>();
        services.AddScoped<IPlayerInfo<HealDoneGeneral, int>, SQLPlayerInfoRepository<HealDoneGeneral, int>>();
        services.AddScoped<IPlayerInfoCount<DamageTaken, int>, SQLPlayerInfoCountRepository<DamageTaken, int>>();
        services.AddScoped<IPlayerInfo<DamageTakenGeneral, int>, SQLPlayerInfoRepository<DamageTakenGeneral, int>>();
        services.AddScoped<IPlayerInfoCount<ResourceRecovery, int>, SQLPlayerInfoCountRepository<ResourceRecovery, int>>();
        services.AddScoped<IPlayerInfo<ResourceRecoveryGeneral, int>, SQLPlayerInfoRepository<ResourceRecoveryGeneral, int>>();
        services.AddScoped<IPlayerInfo<PlayerDeath, int>, SQLPlayerInfoRepository<PlayerDeath, int>>();

        services.AddScoped<ISpecScore<SpecializationScore, int>, SQLSpecScoreRepository<SpecializationScore, int>>();

        var dataProcessingType = configuration.GetSection("Database:DataProcessingType").Value ?? string.Empty;
        switch (dataProcessingType)
        {
            case nameof(DataProcessingType.Default):
                services.AddScoped(typeof(IGenericRepository<,>), typeof(SQLRepository<,>));
                break;
            case nameof(DataProcessingType.StoredProcedure):
                services.AddScoped(typeof(IGenericRepository<,>), typeof(SQLSPRepository<,>));
                break;
            default:
                services.AddScoped(typeof(IGenericRepository<,>), typeof(SQLRepository<,>));
                break;
        }
    }

    private static void FirebaseDatabase(IServiceCollection services)
    {
        services.AddDbContext<FirebaseContext>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(FirebaseRepository<,>));
    }
}
