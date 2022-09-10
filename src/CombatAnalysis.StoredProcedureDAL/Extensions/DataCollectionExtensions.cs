using CombatAnalysis.StoredProcedureDAL.Data;
using CombatAnalysis.StoredProcedureDAL.Entities;
using CombatAnalysis.StoredProcedureDAL.Interfaces;
using CombatAnalysis.StoredProcedureDAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.StoredProcedureDAL.Extensions
{
    public static class DataCollectionExtensions
    {
        public static void RegisterDependenciesDAL(this IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            string connection = configuration.GetConnectionString(connectionName);
            services.AddDbContext<CombatAnalysisContext>(options => options.UseSqlServer(connection));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

            services.AddScoped<IGenericRepository<Combat, int>, GenericRepository<Combat, int>>();
            services.AddScoped<IGenericRepository<CombatPlayer, int>, GenericRepository<CombatPlayer, int>>();
            services.AddScoped<IGenericRepository<DamageDone, int>, GenericRepository<DamageDone, int>>();
            services.AddScoped<IGenericRepository<DamageDoneGeneral, int>, GenericRepository<DamageDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<HealDone, int>, GenericRepository<HealDone, int>>();
            services.AddScoped<IGenericRepository<HealDoneGeneral, int>, GenericRepository<HealDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<DamageTaken, int>, GenericRepository<DamageTaken, int>>();
            services.AddScoped<IGenericRepository<DamageTakenGeneral, int>, GenericRepository<DamageTakenGeneral, int>>();
            services.AddScoped<IGenericRepository<ResourceRecovery, int>, GenericRepository<ResourceRecovery, int>>();
            services.AddScoped<IGenericRepository<ResourceRecoveryGeneral, int>, GenericRepository<ResourceRecoveryGeneral, int>>();
        }
    }
}
