using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services;
using CombatAnalysis.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void BLDependencies(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        services.RegisterDependenciesForDAL(configuration, connectionName);

        services.AddScoped<ISqlContextService, SqlContextService>();

        services.AddScoped<IService<CombatLogDto, int>, CombatLogService>();
        services.AddScoped<IService<CombatLogByUserDto, int>, CombatLogByUserService>();
        services.AddScoped<IService<CombatDto, int>, CombatService>();
        services.AddScoped<IService<CombatPlayerDto, int>, CombatPlayerService>();
        services.AddScoped<IService<PlayerParseInfoDto, int>, PlayerParseInfoService>();
        services.AddScoped<IService<SpecializationScoreDto, int>, SpecializationScoreService>();

        services.AddScoped<IPlayerInfoService<DamageDoneDto, int>, DamageDoneService>();
        services.AddScoped<IPlayerInfoService<DamageDoneGeneralDto, int>, DamageDoneGeneralService>();
        services.AddScoped<IPlayerInfoService<HealDoneDto, int>, HealDoneService>();
        services.AddScoped<IPlayerInfoService<HealDoneGeneralDto, int>, HealDoneGeneralService>();
        services.AddScoped<IPlayerInfoService<DamageTakenDto, int>, DamageTakenService>();
        services.AddScoped<IPlayerInfoService<DamageTakenGeneralDto, int>, DamageTakenGeneralService>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryDto, int>, ResourceRecoveryService>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryGeneralDto, int>, ResourceRecoveryGeneralService>();

        services.AddScoped<ISpecScoreService<SpecializationScoreDto, int>, SpecializationScoreService>();
    }
}
