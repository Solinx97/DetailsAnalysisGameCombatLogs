using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Entities.Community;
using CombatAnalysis.DAL.Entities.Post;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories.Firebase;
using CombatAnalysis.DAL.Repositories.SQL;
using CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.DAL.Extensions;

public static class DataCollectionExtensions
{
    public static void RegisterDependenciesForDAL(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        var databaseName = configuration.GetSection("Database:Name").Value??string.Empty;
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

        services.AddDbContext<SQLContext>(options =>
        {
            options.UseSqlServer(connection);
        });

        services.AddScoped<IUserRepository, SQLUserRepository>();
        services.AddScoped<ITokenRepository<RefreshToken>, SQLRefreshTokenRepository>();
        services.AddScoped<ITokenRepository<AccessToken>, SQLAccessTokenRepository>();
        services.AddScoped<IGenericRepository<Customer, string>, SQLRepository<Customer, string>>();
        services.AddScoped<IGenericRepository<Friend, int>, SQLRepository<Friend, int>>();
        services.AddScoped<IGenericRepository<BannedUser, int>, SQLRepository<BannedUser, int>>();
        services.AddScoped<IGenericRepository<RequestToConnect, int>, SQLRepository<RequestToConnect, int>>();
        services.AddScoped<IGenericRepository<PersonalChat, int>, SQLRepository<PersonalChat, int>>();
        services.AddScoped<IGenericRepository<PersonalChatMessage, int>, SQLRepository<PersonalChatMessage, int>>();
        services.AddScoped<IGenericRepository<InviteToGroupChat, int>, SQLRepository<InviteToGroupChat, int>>();
        services.AddScoped<IGenericRepository<GroupChat, int>, SQLRepository<GroupChat, int>>();
        services.AddScoped<IGenericRepository<GroupChatMessage, int>, SQLRepository<GroupChatMessage, int>>();
        services.AddScoped<IGenericRepository<GroupChatUser, int>, SQLRepository<GroupChatUser, int>>();
        services.AddScoped<IGenericRepository<Post, int>, SQLRepository<Post, int>>();
        services.AddScoped<IGenericRepository<PostDislike, int>, SQLRepository<PostDislike, int>>();
        services.AddScoped<IGenericRepository<PostLike, int>, SQLRepository<PostLike, int>>();
        services.AddScoped<IGenericRepository<PostComment, int>, SQLRepository<PostComment, int>>();
        services.AddScoped<IGenericRepository<CommunityPost, int>, SQLRepository<CommunityPost, int>>();
        services.AddScoped<IGenericRepository<UserPost, int>, SQLRepository<UserPost, int>>();
        services.AddScoped<IGenericRepository<Community, int>, SQLRepository<Community, int>>();
        services.AddScoped<IGenericRepository<CommunityPost, int>, SQLRepository<CommunityPost, int>>();
        services.AddScoped<IGenericRepository<CommunityUser, int>, SQLRepository<CommunityUser, int>>();
        services.AddScoped<IGenericRepository<InviteToCommunity, int>, SQLRepository<InviteToCommunity, int>>();

        var dataProcessingType = configuration.GetSection("Database:DataProcessingType").Value ?? string.Empty;
        switch (dataProcessingType)
        {
            case nameof(DataProcessingType.Default):
                services.AddScoped(typeof(IGenericRepository<,>), typeof(SQLRepository<,>));
                break;
            case nameof(DataProcessingType.StoredProcedure):
                services.AddScoped(typeof(IGenericRepository<,>), typeof(SQLSPRepository<,>));
                break;
            default:
                services.AddScoped(typeof(IGenericRepository<,>), typeof(SQLRepository<,>));
                break;
        }
    }

    private static void FirebaseDatabase(IServiceCollection services)
    {
        services.AddDbContext<FirebaseContext>();

        services.AddScoped<IUserRepository, FIrebaseUserRepository>();
        services.AddScoped<ITokenRepository<RefreshToken>, FirebaseRefreshTokenRepository>();
        services.AddScoped<ITokenRepository<AccessToken>, FirebaseAccessTokenRepository>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(FirebaseRepositroy<,>));
    }
}
