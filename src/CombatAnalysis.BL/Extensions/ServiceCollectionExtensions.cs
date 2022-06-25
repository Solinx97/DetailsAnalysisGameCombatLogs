using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services;
using CombatAnalysis.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.BL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependenciesBL(this IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            services.RegisterDependenciesDAL(configuration, connectionName);

            services.AddScoped<IService<CombatLogDto>, CombatLogService>();
            services.AddScoped<ICombatService<CombatDto>, CombatService>();
            services.AddScoped<IService<CombatPlayerDataDto>, CombatPlayerService>();
            services.AddScoped<IService<DamageDoneDto>, DamageDoneService>();
            services.AddScoped<IService<DamageDoneGeneralDto>, DamageDoneGeneralService>();
            services.AddScoped<IService<HealDoneDto>, HealDoneService>();
            services.AddScoped<IService<HealDoneGeneralDto>, HealDoneGeneralService>();
            services.AddScoped<IService<DamageTakenDto>, DamageTakenService>();
            services.AddScoped<IService<DamageTakenGeneralDto>, DamageTakenGeneralService>();
            services.AddScoped<IService<ResourceRecoveryDto>, ResourceRecoveryService>();
            services.AddScoped<IService<ResourceRecoveryGeneralDto>, ResourceRecoveryGeneralService>();
        }
    }
}
