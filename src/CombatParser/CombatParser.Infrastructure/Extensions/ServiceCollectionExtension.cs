using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Data.Dashboard;
using CombatParser.Domain.Data.Filters;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Entities.WoWMoPClassic;
using CombatParser.Infrastructure.Data;
using CombatParser.Infrastructure.Data.Dashboard;
using CombatParser.Infrastructure.Data.Filters;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatParser.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CombatParserContextOne>(options =>
        {
            options.UseSqlServer(connectionString,
                sql =>
                {
                    sql.CommandTimeout(600);
                });
        });

        services.AddScoped<IGenericRepository<CombatLog, int>, GenericRepository<CombatLog, int>>();
        services.AddScoped<IGenericRepository<BossMap, int>, GenericRepository<BossMap, int>>();
        services.AddScoped<IGenericRepository<SpecializationScore, int>, GenericRepository<SpecializationScore, int>>();
        services.AddScoped<IGenericRepository<BestSpecializationScore, int>, GenericRepository<BestSpecializationScore, int>>();
        services.AddScoped<IGenericRepository<WoWMoPClassicPlayerStats, int>, GenericRepository<WoWMoPClassicPlayerStats, int>>();
        services.AddScoped<IGenericRepository<Player, string>, GenericRepository<Player, string>>();
        services.AddScoped<IGenericRepository<UnitPosition, string>, GenericRepository<UnitPosition, string>>();

        services.AddScoped<ICombatLogRepository, CombatLogRepository>();
        services.AddScoped<ICombatRepository, CombatRepository>();
        services.AddScoped<ICombatPlayerRepository, CombatPlayerRepository>();
        services.AddScoped<IBossRepository, BossRepository>();
        services.AddScoped<ICombatPlayerAuraRepository, CombatPlayerAuraRepository>();
        services.AddScoped<ICombatAbilityRepository, CombatAbilityRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IBestSpecializationScoreRepository, BestSpecializationScoreRepository>();

        services.AddScoped<IUnitRepository, UnitRepository>();

        services.AddScoped<ICombatPlayerDataByTimeRepository<DamageDone>, CombatPlayerDataByTimeRepository<DamageDone>>();
        services.AddScoped<ICombatPlayerDataByTimeRepository<HealDone>, CombatPlayerDataByTimeRepository<HealDone>>();
        services.AddScoped<ICombatPlayerDataByTimeRepository<ResourceRecovery>, CombatPlayerDataByTimeRepository<ResourceRecovery>>();

        services.AddScoped<ICombatPlayerInfoRepository<SpecializationScore>, CombatPlayerInfoRepository<SpecializationScore>>();
        services.AddScoped<ICombatPlayerInfoRepository<WoWMoPClassicPlayerStats>, CombatPlayerInfoRepository<WoWMoPClassicPlayerStats>>();

        services.AddScoped<IUnitInfoRepository<DamageDoneGeneral>, UnitInfoRepository<DamageDoneGeneral>>();
        services.AddScoped<IUnitInfoRepository<HealDoneGeneral>, UnitInfoRepository<HealDoneGeneral>>();
        services.AddScoped<IUnitInfoRepository<ResourceRecoveryGeneral>, UnitInfoRepository<ResourceRecoveryGeneral>>();

        services.AddScoped<IGeneralRepository<DamageDone>, GeneralRepositroy<DamageDone>>();
        services.AddScoped<IGeneralRepository<HealDone>, GeneralRepositroy<HealDone>>();
        services.AddScoped<IGeneralRepository<ResourceRecovery>, GeneralRepositroy<ResourceRecovery>>();

        services.AddScoped<IChartRepository<DamageDone>, ChartRepository<DamageDone>>();
        services.AddScoped<IChartRepository<HealDone>, ChartRepository<HealDone>>();
        services.AddScoped<IChartRepository<ResourceRecovery>, ChartRepository<ResourceRecovery>>();

        services.AddScoped<IDashboardRepository, DashboardRepository>();

        services.AddScoped<ICombatDataRepository<Unit>, CombatDataRepository<Unit>>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
