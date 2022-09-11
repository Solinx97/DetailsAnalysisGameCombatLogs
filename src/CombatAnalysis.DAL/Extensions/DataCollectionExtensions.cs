using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories;
using CombatAnalysis.DAL.Repositories.SQL;
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
            var databaseName = configuration.GetSection("Database:Name").Value;
            switch (databaseName)
            {
                case nameof(DatabaseType.MSSQL):
                    MSSQLDatabase(services, configuration, connectionName);
                    break;
                case nameof(DatabaseType.Firebase):
                    FirebaseDatabase(services, configuration, connectionName);
                    break;
                default:
                    MSSQLDatabase(services, configuration, connectionName);
                    break;
            }
        }

        private static void MSSQLDatabase(IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            var connection = configuration.GetConnectionString(connectionName);

            services.AddDbContext<CombatAnalysisContext>(options => options.UseSqlServer(connection));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

            var dataProcessingType = configuration.GetSection("Database:DataProcessingType").Value;
            switch (dataProcessingType)
            {
                case nameof(DataProcessingType.Default):
                    MSSQLDAL(services);
                    break;
                case nameof(DataProcessingType.StoredProcedure):
                    MSSQLStoredProcedureDAL(services);
                    break;
                default:
                    MSSQLDAL(services);
                    break;
            }
        }

        private static void FirebaseDatabase(IServiceCollection services, IConfiguration configuration, string connectionName)
        {
        }

        public static void MSSQLDAL(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGenericRepository<CombatLog, int>, GenericRepository<CombatLog, int>>();
            services.AddScoped<IGenericRepository<CombatLogByUser, int>, GenericRepository<CombatLogByUser, int>>();
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
            services.AddScoped<ITokenRepository, TokenRepository>();
        }

        public static void MSSQLStoredProcedureDAL(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGenericRepository<CombatLog, int>, SPGenericRepository<CombatLog, int>>();
            services.AddScoped<IGenericRepository<CombatLogByUser, int>, SPGenericRepository<CombatLogByUser, int>>();
            services.AddScoped<IGenericRepository<Combat, int>, SPGenericRepository<Combat, int>>();
            services.AddScoped<IGenericRepository<CombatPlayer, int>, SPGenericRepository<CombatPlayer, int>>();
            services.AddScoped<IGenericRepository<DamageDone, int>, SPGenericRepository<DamageDone, int>>();
            services.AddScoped<IGenericRepository<DamageDoneGeneral, int>, SPGenericRepository<DamageDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<HealDone, int>, SPGenericRepository<HealDone, int>>();
            services.AddScoped<IGenericRepository<HealDoneGeneral, int>, SPGenericRepository<HealDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<DamageTaken, int>, SPGenericRepository<DamageTaken, int>>();
            services.AddScoped<IGenericRepository<DamageTakenGeneral, int>, SPGenericRepository<DamageTakenGeneral, int>>();
            services.AddScoped<IGenericRepository<ResourceRecovery, int>, SPGenericRepository<ResourceRecovery, int>>();
            services.AddScoped<IGenericRepository<ResourceRecoveryGeneral, int>, SPGenericRepository<ResourceRecoveryGeneral, int>>();
            services.AddScoped<ITokenRepository, TokenRepository>();
        }
    }
}
