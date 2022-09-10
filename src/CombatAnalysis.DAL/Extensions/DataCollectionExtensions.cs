using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.DAL.Extensions
{
    public static class DataCollectionExtensions
    {
        public static void RegisterDependenciesDAL(this IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            string connection = configuration.GetConnectionString(connectionName);
            services.AddDbContext<CombatAnalysisContext>(options => options.UseSqlServer(connection));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGenericRepository<CombatLog>, GenericRepository<CombatLog>>();
            services.AddScoped<IGenericRepository<CombatLogByUser>, GenericRepository<CombatLogByUser>>();

            services.AddScoped<IGenericRepository<Combat>, SPGenericRepository<Combat>>();
            services.AddScoped<IGenericRepository<CombatPlayer>, SPGenericRepository<CombatPlayer>>();
            services.AddScoped<IGenericRepository<DamageDone>, SPGenericRepository<DamageDone>>();
            services.AddScoped<IGenericRepository<DamageDoneGeneral>, SPGenericRepository<DamageDoneGeneral>>();
            services.AddScoped<IGenericRepository<HealDone>, SPGenericRepository<HealDone>>();
            services.AddScoped<IGenericRepository<HealDoneGeneral>, SPGenericRepository<HealDoneGeneral>>();
            services.AddScoped<IGenericRepository<DamageTaken>, SPGenericRepository<DamageTaken>>();
            services.AddScoped<IGenericRepository<DamageTakenGeneral>, SPGenericRepository<DamageTakenGeneral>>();
            services.AddScoped<IGenericRepository<ResourceRecovery>, SPGenericRepository<ResourceRecovery>>();
            services.AddScoped<IGenericRepository<ResourceRecoveryGeneral>, SPGenericRepository<ResourceRecoveryGeneral>>();

            services.AddScoped<ISPGenericRepository<Combat>, SPGenericRepository<Combat>>();
            services.AddScoped<ISPGenericRepository<CombatPlayer>, SPGenericRepository<CombatPlayer>>();
            services.AddScoped<ISPGenericRepository<DamageDone>, SPGenericRepository<DamageDone>>();
            services.AddScoped<ISPGenericRepository<DamageDoneGeneral>, SPGenericRepository<DamageDoneGeneral>>();
            services.AddScoped<ISPGenericRepository<HealDone>, SPGenericRepository<HealDone>>();
            services.AddScoped<ISPGenericRepository<HealDoneGeneral>, SPGenericRepository<HealDoneGeneral>>();
            services.AddScoped<ISPGenericRepository<DamageTaken>, SPGenericRepository<DamageTaken>>();
            services.AddScoped<ISPGenericRepository<DamageTakenGeneral>, SPGenericRepository<DamageTakenGeneral>>();
            services.AddScoped<ISPGenericRepository<ResourceRecovery>, SPGenericRepository<ResourceRecovery>>();
            services.AddScoped<ISPGenericRepository<ResourceRecoveryGeneral>, SPGenericRepository<ResourceRecoveryGeneral>>();
            services.AddScoped<ITokenRepository, TokenRepository>();
        }
    }
}
