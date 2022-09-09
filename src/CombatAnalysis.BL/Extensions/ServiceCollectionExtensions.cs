using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.DTO.User;
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

            services.AddScoped<IUserService<UserDto>, UserService>();
            services.AddScoped<IService<CombatLogDto, int>, CombatLogService>();
            services.AddScoped<IService<CombatLogByUserDto, int>, CombatLogByUserService>();
            services.AddScoped<ISPService<CombatDto, int>, CombatService>();
            services.AddScoped<ISPService<CombatPlayerDataDto, int>, CombatPlayerService>();
            services.AddScoped<ISPService<DamageDoneDto, int>, DamageDoneService>();
            services.AddScoped<ISPService<DamageDoneGeneralDto, int>, DamageDoneGeneralService>();
            services.AddScoped<ISPService<HealDoneDto, int>, HealDoneService>();
            services.AddScoped<ISPService<HealDoneGeneralDto, int>, HealDoneGeneralService>();
            services.AddScoped<ISPService<DamageTakenDto, int>, DamageTakenService>();
            services.AddScoped<ISPService<DamageTakenGeneralDto, int>, DamageTakenGeneralService>();
            services.AddScoped<ISPService<ResourceRecoveryDto, int>, ResourceRecoveryService>();
            services.AddScoped<ISPService<ResourceRecoveryGeneralDto, int>, ResourceRecoveryGeneralService>();
        }
    }
}
