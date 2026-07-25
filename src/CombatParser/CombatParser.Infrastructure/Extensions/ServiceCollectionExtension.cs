using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Data.Dashboard;
using CombatParser.Domain.Data.Filters;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
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
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IGenericRepository<CombatLog, int>, GenericRepository<CombatLog, int>>();
        services.AddScoped<IGenericRepository<BossMap, int>, GenericRepository<BossMap, int>>();
        services.AddScoped<IGenericRepository<SpecializationScore, int>, GenericRepository<SpecializationScore, int>>();
        services.AddScoped<IGenericRepository<BestSpecializationScore, int>, GenericRepository<BestSpecializationScore, int>>();
        services.AddScoped<IGenericRepository<CombatPlayerStats, int>, GenericRepository<CombatPlayerStats, int>>();
        services.AddScoped<IGenericRepository<Player, string>, GenericRepository<Player, string>>();
        services.AddScoped<IGenericRepository<CombatPlayerPosition, string>, GenericRepository<CombatPlayerPosition, string>>();

        services.AddScoped<ICombatLogRepository, CombatLogRepository>();
        services.AddScoped<ICombatRepository, CombatRepository>();
        services.AddScoped<ICombatPlayerRepository, CombatPlayerRepository>();
        services.AddScoped<IBossRepository, BossRepository>();
        services.AddScoped<ICombatPlayerAuraRepository, CombatPlayerAuraRepository>();
        services.AddScoped<ICombatAbilityRepository, CombatAbilityRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IBestSpecializationScoreRepository, BestSpecializationScoreRepository>();
        services.AddScoped<ICombatPlayerPositionRepository, CombatPlayerPositionRepository>();

        services.AddScoped<ICombatPlayerDataByTimeRepository<DamageDone>, CombatPlayerDataByTimeRepository<DamageDone>>();
        services.AddScoped<ICombatPlayerDataByTimeRepository<HealDone>, CombatPlayerDataByTimeRepository<HealDone>>();
        services.AddScoped<ICombatPlayerDataByTimeRepository<DamageTaken>, CombatPlayerDataByTimeRepository<DamageTaken>>();
        services.AddScoped<ICombatPlayerDataByTimeRepository<ResourceRecovery>, CombatPlayerDataByTimeRepository<ResourceRecovery>>();
        services.AddScoped<ICombatPlayerDataByTimeRepository<CombatPlayerDeath>, CombatPlayerDataByTimeRepository<CombatPlayerDeath>>();

        services.AddScoped<ICombatPlayerInfoRepository<SpecializationScore>, CombatPlayerInfoRepository<SpecializationScore>>();
        services.AddScoped<ICombatPlayerInfoRepository<CombatPlayerStats>, CombatPlayerInfoRepository<CombatPlayerStats>>();
        services.AddScoped<ICombatPlayerInfoRepository<CombatPlayerCast>, CombatPlayerInfoRepository<CombatPlayerCast>>();
        services.AddScoped<ICombatPlayerInfoRepository<DamageDoneGeneral>, CombatPlayerInfoRepository<DamageDoneGeneral>>();
        services.AddScoped<ICombatPlayerInfoRepository<HealDoneGeneral>, CombatPlayerInfoRepository<HealDoneGeneral>>();
        services.AddScoped<ICombatPlayerInfoRepository<DamageTakenGeneral>, CombatPlayerInfoRepository<DamageTakenGeneral>>();
        services.AddScoped<ICombatPlayerInfoRepository<ResourceRecoveryGeneral>, CombatPlayerInfoRepository<ResourceRecoveryGeneral>>();

        services.AddScoped<IGeneralRepository<DamageDone>, GeneralRepositroy<DamageDone>>();
        services.AddScoped<IGeneralRepository<HealDone>, GeneralRepositroy<HealDone>>();
        services.AddScoped<IGeneralRepository<DamageTaken>, GeneralRepositroy<DamageTaken>>();
        services.AddScoped<IGeneralRepository<ResourceRecovery>, GeneralRepositroy<ResourceRecovery>>();

        services.AddScoped<IChartRepository<DamageDone>, ChartRepository<DamageDone>>();
        services.AddScoped<IChartRepository<HealDone>, ChartRepository<HealDone>>();
        services.AddScoped<IChartRepository<DamageTaken>, ChartRepository<DamageTaken>>();
        services.AddScoped<IChartRepository<ResourceRecovery>, ChartRepository<ResourceRecovery>>();

        services.AddScoped<IDashboardRepository, DashboardRepository>();

        services.AddScoped<ICombatDataRepository<UnitHealth>, CombatDataRepository<UnitHealth>>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
