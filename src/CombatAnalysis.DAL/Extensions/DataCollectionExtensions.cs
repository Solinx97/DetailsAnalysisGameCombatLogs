using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Data.SQL;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories.Firebase;
using CombatAnalysis.DAL.Repositories.SQL;
using CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;
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
                    FirebaseDatabase(services);
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

        private static void FirebaseDatabase(IServiceCollection services)
        {
            services.AddDbContext<FirebaseContext>();

            services.AddScoped<IUserRepository, FIrebaseUserRepository>();
            services.AddScoped<ITokenRepository, FirebaseTokenRepository>();

            services.AddScoped<IGenericRepository<PersonalChat, int>, FirebaseRepositroy<PersonalChat, int>>();
            services.AddScoped<IGenericRepository<PersonalChatMessage, int>, FirebaseRepositroy<PersonalChatMessage, int>>();
            services.AddScoped<IGenericRepository<InviteToGroupChat, int>, FirebaseRepositroy<InviteToGroupChat, int>>();
            services.AddScoped<IGenericRepository<GroupChat, int>, FirebaseRepositroy<GroupChat, int>>();
            services.AddScoped<IGenericRepository<GroupChatMessage, int>, FirebaseRepositroy<GroupChatMessage, int>>();
            services.AddScoped<IGenericRepository<GroupChatUser, int>, FirebaseRepositroy<GroupChatUser, int>>();
            services.AddScoped<IGenericRepository<BannedUser, int>, FirebaseRepositroy<BannedUser, int>>();

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
        }

        public static void MSSQLDAL(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, SQLUserRepository>();
            services.AddScoped<ITokenRepository, SQLTokenRepository>();

            services.AddScoped<IGenericRepository<PersonalChat, int>, SQLRepository<PersonalChat, int>>();
            services.AddScoped<IGenericRepository<PersonalChatMessage, int>, SQLRepository<PersonalChatMessage, int>>();
            services.AddScoped<IGenericRepository<InviteToGroupChat, int>, SQLRepository<InviteToGroupChat, int>>();
            services.AddScoped<IGenericRepository<GroupChat, int>, SQLRepository<GroupChat, int>>();
            services.AddScoped<IGenericRepository<GroupChatMessage, int>, SQLRepository<GroupChatMessage, int>>();
            services.AddScoped<IGenericRepository<GroupChatUser, int>, SQLRepository<GroupChatUser, int>>();
            services.AddScoped<IGenericRepository<BannedUser, int>, SQLRepository<BannedUser, int>>();

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
        }

        public static void MSSQLStoredProcedureDAL(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, SQLSPUserRepository>();
            services.AddScoped<ITokenRepository, SQLSPTokenRepository>();

            services.AddScoped<IGenericRepository<PersonalChat, int>, SQLSPRepository<PersonalChat, int>>();
            services.AddScoped<IGenericRepository<PersonalChatMessage, int>, SQLSPRepository<PersonalChatMessage, int>>();
            services.AddScoped<IGenericRepository<InviteToGroupChat, int>, SQLSPRepository<InviteToGroupChat, int>>();
            services.AddScoped<IGenericRepository<GroupChat, int>, SQLSPRepository<GroupChat, int>>();
            services.AddScoped<IGenericRepository<GroupChatMessage, int>, SQLSPRepository<GroupChatMessage, int>>();
            services.AddScoped<IGenericRepository<GroupChatUser, int>, SQLSPRepository<GroupChatUser, int>>();
            services.AddScoped<IGenericRepository<BannedUser, int>, SQLSPRepository<BannedUser, int>>();

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
        }
    }
}
