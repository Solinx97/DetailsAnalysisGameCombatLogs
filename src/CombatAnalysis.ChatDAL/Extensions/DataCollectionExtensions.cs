using CombatAnalysis.ChatDAL.Data;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Enums;
using CombatAnalysis.ChatDAL.Interfaces;
using CombatAnalysis.ChatDAL.Repositories.Firebase;
using CombatAnalysis.ChatDAL.Repositories.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.ChatDAL.Extensions;

public static class DataCollectionExtensions
{
    public static void RegisterDependenciesForDAL(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        var databaseName = configuration.GetSection("Database:Name").Value ?? string.Empty;
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

        services.AddDbContext<ChatSQLContext>(options =>
        {
            options.UseSqlServer(connection);
        });

        services.AddScoped<IGenericRepository<VoiceChat, string>, SQLRepository<VoiceChat, string>>();
        services.AddScoped<IGenericRepository<PersonalChat, int>, SQLRepository<PersonalChat, int>>();
        services.AddScoped<IGenericRepository<PersonalChatMessage, int>, SQLRepository<PersonalChatMessage, int>>();
        services.AddScoped<IGenericRepository<PersonalChatMessageCount, int>, SQLRepository<PersonalChatMessageCount, int>>();
        services.AddScoped<IGenericRepository<GroupChat, int>, SQLRepository<GroupChat, int>>();
        services.AddScoped<IGenericRepository<GroupChatRules, int>, SQLRepository<GroupChatRules, int>>();
        services.AddScoped<IGenericRepository<GroupChatMessage, int>, SQLRepository<GroupChatMessage, int>>();
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
