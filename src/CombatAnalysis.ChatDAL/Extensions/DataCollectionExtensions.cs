using CombatAnalysis.ChatDAL.Data;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Enums;
using CombatAnalysis.ChatDAL.Interfaces;
using CombatAnalysis.ChatDAL.Repositories.Firebase;
using CombatAnalysis.ChatDAL.Repositories.SQL;
using CombatAnalysis.ChatDAL.Repositories.SQL.StoredProcedure;
using CombatAnalysis.ChatDAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.ChatDAL.Extensions;

public static class DataCollectionExtensions
{
    public static void RegisterDependenciesForDAL(this IServiceCollection services, string databaseName, string dataProcessingType, string connectionString)
    {
        switch (databaseName)
        {
            case nameof(DatabaseType.MSSQL):
                MSSQLDatabase(services, connectionString);
                break;
            case nameof(DatabaseType.Firebase):
                FirebaseDatabase(services);
                break;
            default:
                MSSQLDatabase(services, connectionString);
                break;
        }
    }

    private static void MSSQLDatabase(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ChatSQLContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IGenericRepository<VoiceChat, string>, SQLRepository<VoiceChat, string>>();
        services.AddScoped<IGenericRepository<PersonalChat, int>, SQLRepository<PersonalChat, int>>();
        services.AddScoped<IChatMessageRepository<PersonalChatMessage, int>, SQLSPChatMessageRepository<PersonalChatMessage, int>>();
        services.AddScoped<IGenericRepository<PersonalChatMessageCount, int>, SQLRepository<PersonalChatMessageCount, int>>();
        services.AddScoped<IGenericRepository<GroupChat, int>, SQLRepository<GroupChat, int>>();
        services.AddScoped<IGenericRepository<GroupChatRules, int>, SQLRepository<GroupChatRules, int>>();
        services.AddScoped<IChatMessageRepository<GroupChatMessage, int>, SQLSPChatMessageRepository<GroupChatMessage, int>>();
        services.AddScoped<IGenericRepository<UnreadGroupChatMessage, int>, SQLRepository<UnreadGroupChatMessage, int>>();
        services.AddScoped<IGenericRepository<GroupChatMessageCount, int>, SQLRepository<GroupChatMessageCount, int>>();
        services.AddScoped<IGenericRepository<GroupChatUser, string>, SQLRepository<GroupChatUser, string>>();
    }

    private static void FirebaseDatabase(IServiceCollection services)
    {
        services.AddDbContext<FirebaseContext>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(FirebaseRepository<,>));
    }
}
