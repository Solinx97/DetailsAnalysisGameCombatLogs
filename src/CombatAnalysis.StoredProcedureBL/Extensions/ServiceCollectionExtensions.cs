using CombatAnalysis.StoredProcedureBL.DTO;
using CombatAnalysis.StoredProcedureBL.DTO.User;
using CombatAnalysis.StoredProcedureBL.Interfaces;
using CombatAnalysis.StoredProcedureBL.Services;
using CombatAnalysis.StoredProcedureDAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.StoredProcedureBL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependenciesBL(this IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            services.RegisterDependenciesDAL(configuration, connectionName);

            services.AddScoped<IService<UserDto, string>, UserService>();
            services.AddScoped<IService<CombatLogDto, int>, CombatLogService>();
            services.AddScoped<IService<CombatLogByUserDto, int>, CombatLogByUserService>();
            services.AddScoped<IService<CombatDto, int>, CombatService>();
            services.AddScoped<IService<CombatPlayerDto, int>, CombatPlayerService>();
            services.AddScoped<IService<DamageDoneDto, int>, DamageDoneService>();
            services.AddScoped<IService<DamageDoneGeneralDto, int>, DamageDoneGeneralService>();
            services.AddScoped<IService<HealDoneDto, int>, HealDoneService>();
            services.AddScoped<IService<HealDoneGeneralDto, int>, HealDoneGeneralService>();
            services.AddScoped<IService<DamageTakenDto, int>, DamageTakenService>();
            services.AddScoped<IService<DamageTakenGeneralDto, int>, DamageTakenGeneralService>();
            services.AddScoped<IService<ResourceRecoveryDto, int>, ResourceRecoveryService>();
            services.AddScoped<IService<ResourceRecoveryGeneralDto, int>, ResourceRecoveryGeneralService>();
        }
    }
}
