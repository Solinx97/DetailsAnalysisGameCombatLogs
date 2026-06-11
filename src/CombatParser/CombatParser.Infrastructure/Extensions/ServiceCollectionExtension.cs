using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Data.Filters;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces.Filters;
using CombatParser.Infrastructure.Data;
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
        services.AddScoped<IGenericRepository<Combat, int>, GenericRepository<Combat, int>>();
        services.AddScoped<IGenericRepository<SpecializationScore, int>, GenericRepository<SpecializationScore, int>>();
        services.AddScoped<IGenericRepository<BestSpecializationScore, int>, GenericRepository<BestSpecializationScore, int>>();
        services.AddScoped<IGenericRepository<CombatPlayerStats, int>, GenericRepository<CombatPlayerStats, int>>();
        services.AddScoped<IGenericRepository<CombatPlayerPosition, int>, GenericRepository<CombatPlayerPosition, int>>();
        services.AddScoped<IGenericRepository<Player, string>, GenericRepository<Player, string>>();

        services.AddScoped<ICombatLogRepository, CombatLogRepository>();
        services.AddScoped<ICombatRepository, CombatRepository>();
        services.AddScoped<ICombatPlayerRepository, CombatPlayerRepository>();
        services.AddScoped<IBossRepository, BossRepository>();
        services.AddScoped<ICombatPlayerAuraRepository, CombatPlayerAuraRepository>();
        services.AddScoped<ICombatAbilityRepository, CombatAbilityRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IBestSpecializationScoreRepository, BestSpecializationScoreRepository>();

        services.AddScoped<ICombatPlayerDataRepository<DamageDone>, CombatPlayerDataRepository<DamageDone>>();
        services.AddScoped<ICombatPlayerDataRepository<HealDone>, CombatPlayerDataRepository<HealDone>>();
        services.AddScoped<ICombatPlayerDataRepository<DamageTaken>, CombatPlayerDataRepository<DamageTaken>>();
        services.AddScoped<ICombatPlayerDataRepository<ResourceRecovery>, CombatPlayerDataRepository<ResourceRecovery>>();
        services.AddScoped<ICombatPlayerDataRepository<CombatPlayerPosition>, CombatPlayerDataRepository<CombatPlayerPosition>>();
        services.AddScoped<ICombatPlayerDataRepository<CombatPlayerDeath>, CombatPlayerDataRepository<CombatPlayerDeath>>();
        services.AddScoped<ICombatPlayerDataRepository<SpecializationScore>, CombatPlayerDataRepository<SpecializationScore>>();
        services.AddScoped<ICombatPlayerDataRepository<CombatPlayerStats>, CombatPlayerDataRepository<CombatPlayerStats>>();

        services.AddScoped<IGeneralFilterRepository<DamageDone>, GeneralFilterRepositroy<DamageDone>>();
        services.AddScoped<IGeneralFilterRepository<HealDone>, GeneralFilterRepositroy<HealDone>>();
        services.AddScoped<IGeneralFilterRepository<DamageTaken>, GeneralFilterRepositroy<DamageTaken>>();
        services.AddScoped<IGeneralFilterRepository<ResourceRecovery>, GeneralFilterRepositroy<ResourceRecovery>>();

        services.AddScoped<IDamageFilterRepository, DamageFilterRepository>();

        services.AddScoped<ICombatPlayerGenericDataRepository<DamageDoneGeneral>, CombatPlayerGenericDataRepository<DamageDoneGeneral>>();
        services.AddScoped<ICombatPlayerGenericDataRepository<HealDoneGeneral>, CombatPlayerGenericDataRepository<HealDoneGeneral>>();
        services.AddScoped<ICombatPlayerGenericDataRepository<DamageTakenGeneral>, CombatPlayerGenericDataRepository<DamageTakenGeneral>>();
        services.AddScoped<ICombatPlayerGenericDataRepository<ResourceRecoveryGeneral>, CombatPlayerGenericDataRepository<ResourceRecoveryGeneral>>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
