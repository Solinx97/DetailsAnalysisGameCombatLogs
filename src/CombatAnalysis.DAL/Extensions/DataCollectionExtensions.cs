using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories;
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

            services.AddScoped<IGenericRepository<CombatLog>, GenericRepository<CombatLog>>();
            services.AddScoped<IGenericRepository<Combat>, GenericRepository<Combat>>();
            services.AddScoped<IGenericRepository<CombatPlayerData>, GenericRepository<CombatPlayerData>>();
            services.AddScoped<IGenericRepository<DamageDone>, GenericRepository<DamageDone>>();
            services.AddScoped<IGenericRepository<DamageDoneGeneral>, GenericRepository<DamageDoneGeneral>>();
            services.AddScoped<IGenericRepository<HealDone>, GenericRepository<HealDone>>();
            services.AddScoped<IGenericRepository<DamageTaken>, GenericRepository<DamageTaken>>();
            services.AddScoped<IGenericRepository<ResourceRecovery>, GenericRepository<ResourceRecovery>>();
        }
    }
}
