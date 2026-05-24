using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Data.Filters;
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
        services.AddScoped<ICombatLogRepository, CombatLogRepository>();
        services.AddScoped<ICombatRepository, CombatRepository>();
        services.AddScoped<ICombatPlayerRepository, CombatPlayerRepository>();
        services.AddScoped<IBossRepository, BossRepository>();
        services.AddScoped<ICombatPlayerAuraRepository, CombatPlayerAuraRepository>();
        services.AddScoped<ICombatAbilityRepository, CombatAbilityRepository>();

        services.AddScoped<ICombatPlayerDataRepository<DamageDone>, CombatPlayerDataRepository<DamageDone>>();
        services.AddScoped<ICombatPlayerDataRepository<HealDone>, CombatPlayerDataRepository<HealDone>>();
        services.AddScoped<ICombatPlayerDataRepository<DamageTaken>, CombatPlayerDataRepository<DamageTaken>>();
        services.AddScoped<ICombatPlayerDataRepository<ResourceRecovery>, CombatPlayerDataRepository<ResourceRecovery>>();

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
