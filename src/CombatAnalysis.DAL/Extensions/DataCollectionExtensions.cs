using CombatAnalysis.DAL.Consts;
using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Filters;
using CombatAnalysis.DAL.Interfaces.Generic;
using CombatAnalysis.DAL.Repositories.Firebase;
using CombatAnalysis.DAL.Repositories.SQL;
using CombatAnalysis.DAL.Repositories.SQL.Filters;
using CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;
using CombatAnalysis.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.DAL.Extensions;

public static class DataCollectionExtensions
{
    public static void CombatParserDALDependencies(this IServiceCollection services, string databaseName, string dataProcessingType, string connectionString, int commandTimeout)
    {
        DBConfigurations.CommandTimeout = commandTimeout;

        switch (databaseName)
        {
            case nameof(DatabaseType.MSSQL):
                MSSQLDatabase(services, dataProcessingType, connectionString);
                break;
            case nameof(DatabaseType.Firebase):
                FirebaseDatabase(services);
                break;
            default:
                MSSQLDatabase(services, dataProcessingType, connectionString);
                break;
        }
    }

    private static void MSSQLDatabase(IServiceCollection services, string dataProcessingType, string connectionString)
    {
        services.AddDbContext<CombatParserSQLContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IPlayerInfoRepository<Combat>, SQLSPPlayerInfoRepository<Combat>>();
        services.AddScoped<ICountRepository<DamageDone>, SQLCountRepository<DamageDone>>();
        services.AddScoped<IGeneralFilter<DamageDone>, GeneralFilterRepositroy<DamageDone>>();
        services.AddScoped<IPlayerInfoRepository<DamageDoneGeneral>, SQLSPPlayerInfoRepository<DamageDoneGeneral>>();
        services.AddScoped<IPlayerInfoRepository<DamageDone>, SQLSPPlayerInfoRepository<DamageDone>>();
        services.AddScoped<ICountRepository<HealDone>, SQLCountRepository<HealDone>>();
        services.AddScoped<IGeneralFilter<HealDone>, GeneralFilterRepositroy<HealDone>>();
        services.AddScoped<IPlayerInfoRepository<HealDoneGeneral>, SQLSPPlayerInfoRepository<HealDoneGeneral>>();
        services.AddScoped<IPlayerInfoRepository<HealDone>, SQLSPPlayerInfoRepository<HealDone>>();
        services.AddScoped<ICountRepository<DamageTaken>, SQLCountRepository<DamageTaken>>();
        services.AddScoped<IGeneralFilter<DamageTaken>, GeneralFilterRepositroy<DamageTaken>>();
        services.AddScoped<IPlayerInfoRepository<DamageTakenGeneral>, SQLSPPlayerInfoRepository<DamageTakenGeneral>>();
        services.AddScoped<IPlayerInfoRepository<DamageTaken>, SQLSPPlayerInfoRepository<DamageTaken>>();
        services.AddScoped<ICountRepository<ResourceRecovery>, SQLCountRepository<ResourceRecovery>>();
        services.AddScoped<IGeneralFilter<ResourceRecovery>, GeneralFilterRepositroy<ResourceRecovery>>();
        services.AddScoped<IPlayerInfoRepository<ResourceRecoveryGeneral>, SQLSPPlayerInfoRepository<ResourceRecoveryGeneral>>();
        services.AddScoped<IPlayerInfoRepository<ResourceRecovery>, SQLSPPlayerInfoRepository<ResourceRecovery>>();
        services.AddScoped<IPlayerInfoRepository<PlayerDeath>, SQLSPPlayerInfoRepository<PlayerDeath>>();

        services.AddScoped<ISpecScore, SQLSPSpecScoreRepository>();

        switch (dataProcessingType)
        {
            case nameof(DataProcessingType.Default):
                services.AddScoped(typeof(IGenericRepository<>), typeof(SQLRepository<>));
                break;
            case nameof(DataProcessingType.StoredProcedure):
                services.AddScoped(typeof(IGenericRepository<>), typeof(SQLSPRepository<>));
                break;
            default:
                services.AddScoped(typeof(IGenericRepository<>), typeof(SQLRepository<>));
                break;
        }
    }

    private static void FirebaseDatabase(IServiceCollection services)
    {
        services.AddDbContext<FirebaseContext>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(FirebaseRepository<>));
    }
}
