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
            services.AddScoped<IGenericRepository<Combat>, GenericRepository<Combat>>();
            services.AddScoped<IGenericRepository<CombatPlayer>, GenericRepository<CombatPlayer>>();
            services.AddScoped<IGenericRepository<DamageDone>, GenericRepository<DamageDone>>();
            services.AddScoped<IGenericRepository<DamageDoneGeneral>, GenericRepository<DamageDoneGeneral>>();
            services.AddScoped<IGenericRepository<HealDone>, GenericRepository<HealDone>>();
            services.AddScoped<IGenericRepository<HealDoneGeneral>, GenericRepository<HealDoneGeneral>>();
            services.AddScoped<IGenericRepository<DamageTaken>, GenericRepository<DamageTaken>>();
            services.AddScoped<IGenericRepository<DamageTakenGeneral>, GenericRepository<DamageTakenGeneral>>();
            services.AddScoped<IGenericRepository<ResourceRecovery>, GenericRepository<ResourceRecovery>>();
            services.AddScoped<IGenericRepository<ResourceRecoveryGeneral>, GenericRepository<ResourceRecoveryGeneral>>();
            services.AddScoped<ITokenRepository, TokenRepository>();
        }
    }
}
