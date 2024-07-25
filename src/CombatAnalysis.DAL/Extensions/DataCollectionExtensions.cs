using CombatAnalysis.DAL.Consts;
using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories.Firebase;
using CombatAnalysis.DAL.Repositories.SQL;
using CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;
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

        services.AddScoped<ISQLPlayerInfoRepository<Combat, int>, SQLPlayerInfoRepository<Combat, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<DamageDone, int>, SQLPlayerInfoRepository<DamageDone, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<DamageDoneGeneral, int>, SQLPlayerInfoRepository<DamageDoneGeneral, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<HealDone, int>, SQLPlayerInfoRepository<HealDone, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<HealDoneGeneral, int>, SQLPlayerInfoRepository<HealDoneGeneral, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<DamageTaken, int>, SQLPlayerInfoRepository<DamageTaken, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<DamageTakenGeneral, int>, SQLPlayerInfoRepository<DamageTakenGeneral, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<ResourceRecovery, int>, SQLPlayerInfoRepository<ResourceRecovery, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<ResourceRecoveryGeneral, int>, SQLPlayerInfoRepository<ResourceRecoveryGeneral, int>>();
        services.AddScoped<ISQLPlayerInfoRepository<PlayerDeath, int>, SQLPlayerInfoRepository<PlayerDeath, int>>();

        services.AddScoped<ISQLSpecScoreRepository<SpecializationScore, int>, SQLSpecScoreRepository<SpecializationScore, int>>();

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
