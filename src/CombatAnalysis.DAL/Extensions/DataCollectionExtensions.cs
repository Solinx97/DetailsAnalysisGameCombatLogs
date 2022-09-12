using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories.Firebase;
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

            services.AddDbContext<SQLContext>(options => options.UseSqlServer(connection));
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
            services.AddDbContext<FirebaseContext>();

            services.AddScoped<IUserRepository, FIrebaseUserRepository>();
            services.AddScoped<IGenericRepository<CombatLog, int>, FirebaseRepositroy<CombatLog, int>>();
            services.AddScoped<IGenericRepository<CombatLogByUser, int>, FirebaseRepositroy<CombatLogByUser, int>>();
            services.AddScoped<IGenericRepository<Combat, int>, FirebaseRepositroy<Combat, int>>();
            services.AddScoped<IGenericRepository<CombatPlayer, int>, FirebaseRepositroy<CombatPlayer, int>>();
            services.AddScoped<IGenericRepository<DamageDone, int>, FirebaseRepositroy<DamageDone, int>>();
            services.AddScoped<IGenericRepository<DamageDoneGeneral, int>, FirebaseRepositroy<DamageDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<HealDone, int>, FirebaseRepositroy<HealDone, int>>();
            services.AddScoped<IGenericRepository<HealDoneGeneral, int>, FirebaseRepositroy<HealDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<DamageTaken, int>, FirebaseRepositroy<DamageTaken, int>>();
            services.AddScoped<IGenericRepository<DamageTakenGeneral, int>, FirebaseRepositroy<DamageTakenGeneral, int>>();
            services.AddScoped<IGenericRepository<ResourceRecovery, int>, FirebaseRepositroy<ResourceRecovery, int>>();
            services.AddScoped<IGenericRepository<ResourceRecoveryGeneral, int>, FirebaseRepositroy<ResourceRecoveryGeneral, int>>();
            services.AddScoped<ITokenRepository, FirebaseTokenRepository>();
        }

        public static void MSSQLDAL(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, SQLUserRepository>();
            services.AddScoped<IGenericRepository<CombatLog, int>, SQLRepository<CombatLog, int>>();
            services.AddScoped<IGenericRepository<CombatLogByUser, int>, SQLRepository<CombatLogByUser, int>>();
            services.AddScoped<IGenericRepository<Combat, int>, SQLRepository<Combat, int>>();
            services.AddScoped<IGenericRepository<CombatPlayer, int>, SQLRepository<CombatPlayer, int>>();
            services.AddScoped<IGenericRepository<DamageDone, int>, SQLRepository<DamageDone, int>>();
            services.AddScoped<IGenericRepository<DamageDoneGeneral, int>, SQLRepository<DamageDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<HealDone, int>, SQLRepository<HealDone, int>>();
            services.AddScoped<IGenericRepository<HealDoneGeneral, int>, SQLRepository<HealDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<DamageTaken, int>, SQLRepository<DamageTaken, int>>();
            services.AddScoped<IGenericRepository<DamageTakenGeneral, int>, SQLRepository<DamageTakenGeneral, int>>();
            services.AddScoped<IGenericRepository<ResourceRecovery, int>, SQLRepository<ResourceRecovery, int>>();
            services.AddScoped<IGenericRepository<ResourceRecoveryGeneral, int>, SQLRepository<ResourceRecoveryGeneral, int>>();
            services.AddScoped<ITokenRepository, SQLTokenRepository>();
        }

        public static void MSSQLStoredProcedureDAL(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, SQLUserRepository>();
            services.AddScoped<IGenericRepository<CombatLog, int>, SQLSPRepository<CombatLog, int>>();
            services.AddScoped<IGenericRepository<CombatLogByUser, int>, SQLSPRepository<CombatLogByUser, int>>();
            services.AddScoped<IGenericRepository<Combat, int>, SQLSPRepository<Combat, int>>();
            services.AddScoped<IGenericRepository<CombatPlayer, int>, SQLSPRepository<CombatPlayer, int>>();
            services.AddScoped<IGenericRepository<DamageDone, int>, SQLSPRepository<DamageDone, int>>();
            services.AddScoped<IGenericRepository<DamageDoneGeneral, int>, SQLSPRepository<DamageDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<HealDone, int>, SQLSPRepository<HealDone, int>>();
            services.AddScoped<IGenericRepository<HealDoneGeneral, int>, SQLSPRepository<HealDoneGeneral, int>>();
            services.AddScoped<IGenericRepository<DamageTaken, int>, SQLSPRepository<DamageTaken, int>>();
            services.AddScoped<IGenericRepository<DamageTakenGeneral, int>, SQLSPRepository<DamageTakenGeneral, int>>();
            services.AddScoped<IGenericRepository<ResourceRecovery, int>, SQLSPRepository<ResourceRecovery, int>>();
            services.AddScoped<IGenericRepository<ResourceRecoveryGeneral, int>, SQLSPRepository<ResourceRecoveryGeneral, int>>();
            services.AddScoped<ITokenRepository, SQLTokenRepository>();
        }
    }
}
