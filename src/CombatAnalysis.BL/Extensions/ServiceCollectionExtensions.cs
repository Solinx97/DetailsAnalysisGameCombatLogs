using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services;
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

        services.AddScoped<IService<CombatLogDto, int>, CombatLogService>();
        services.AddScoped<IService<CombatLogByUserDto, int>, CombatLogByUserService>();
        services.AddScoped<IService<CombatDto, int>, CombatService>();
        services.AddScoped<IService<CombatPlayerDto, int>, CombatPlayerService>();
        services.AddScoped<IService<PlayerParseInfoDto, int>, PlayerParseInfoService>();

        services.AddScoped<IService<CombatPlayerPositionDto, int>, CombatPlayerPositionService>();
        services.AddScoped<IPlayerInfoService<DamageDoneDto, int>, DamageDoneService>();
        services.AddScoped<IPlayerInfoService<DamageDoneGeneralDto, int>, DamageDoneGeneralService>();
        services.AddScoped<IPlayerInfoService<HealDoneDto, int>, HealDoneService>();
        services.AddScoped<IPlayerInfoService<HealDoneGeneralDto, int>, HealDoneGeneralService>();
        services.AddScoped<IPlayerInfoService<DamageTakenDto, int>, DamageTakenService>();
        services.AddScoped<IPlayerInfoService<DamageTakenGeneralDto, int>, DamageTakenGeneralService>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryDto, int>, ResourceRecoveryService>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryGeneralDto, int>, ResourceRecoveryGeneralService>();
        services.AddScoped<IPlayerInfoService<PlayerDeathDto, int>, PlayerDeathService>();

        services.AddScoped<IPlayerInfoCountService<DamageDoneDto, int>, DamageDoneService>();
        services.AddScoped<IPlayerInfoCountService<HealDoneDto, int>, HealDoneService>();
        services.AddScoped<IPlayerInfoCountService<DamageTakenDto, int>, DamageTakenService>();
        services.AddScoped<IPlayerInfoCountService<ResourceRecoveryDto, int>, ResourceRecoveryService>();

        services.AddScoped<ISpecScoreService<SpecializationScoreDto, int>, SpecializationScoreService>();
    }
}
