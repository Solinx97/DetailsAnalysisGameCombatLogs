using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Services.Filters;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void CombatParserBLDependencies(this IServiceCollection services, IConfiguration configuration, string connectionName, int commandTimeout)
    {
        services.CombatParserDALDependencies(configuration, connectionName, commandTimeout);

        services.AddScoped<ICombatTransactionService, CombatTransactionService>();

        services.AddScoped<IService<CombatLogDto>, CombatLogService>();
        services.AddScoped<IService<CombatLogByUserDto>, CombatLogByUserService>();
        services.AddScoped<IService<CombatDto>, CombatService>();
        services.AddScoped<IService<CombatPlayerDto>, CombatPlayerService>();
        services.AddScoped<IService<CombatAuraDto>, CombatAuraService>();
        services.AddScoped<IService<PlayerParseInfoDto>, PlayerParseInfoService>();

        services.AddScoped<IService<CombatPlayerPositionDto>, CombatPlayerPositionService>();
        services.AddScoped<IPlayerInfoService<DamageDoneDto>, DamageDoneService>();
        services.AddScoped<IGeneralFilterService<DamageDoneDto>, GeneralFilterService<DamageDoneDto, DamageDone>>();
        services.AddScoped<IPlayerInfoService<DamageDoneGeneralDto>, DamageDoneGeneralService>();
        services.AddScoped<IPlayerInfoService<HealDoneDto>, HealDoneService>();
        services.AddScoped<IGeneralFilterService<HealDoneDto>, GeneralFilterService<HealDoneDto, HealDone>>();
        services.AddScoped<IPlayerInfoService<HealDoneGeneralDto>, HealDoneGeneralService>();
        services.AddScoped<IPlayerInfoService<DamageTakenDto>, DamageTakenService>();
        services.AddScoped<IGeneralFilterService<DamageTakenDto>, GeneralFilterService<DamageTakenDto, DamageTaken>>();
        services.AddScoped<IPlayerInfoService<DamageTakenGeneralDto>, DamageTakenGeneralService>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryDto>, ResourceRecoveryService>();
        services.AddScoped<IGeneralFilterService<ResourceRecoveryDto>, GeneralFilterService<ResourceRecoveryDto, ResourceRecovery>>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryGeneralDto>, ResourceRecoveryGeneralService>();
        services.AddScoped<IPlayerInfoService<PlayerDeathDto>, PlayerDeathService>();

        services.AddScoped<IPlayerInfoCountService<DamageDoneDto>, DamageDoneService>();
        services.AddScoped<IPlayerInfoCountService<HealDoneDto>, HealDoneService>();
        services.AddScoped<IPlayerInfoCountService<DamageTakenDto>, DamageTakenService>();
        services.AddScoped<IPlayerInfoCountService<ResourceRecoveryDto>, ResourceRecoveryService>();

        services.AddScoped<ISpecScoreService, SpecializationScoreService>();
    }
}
