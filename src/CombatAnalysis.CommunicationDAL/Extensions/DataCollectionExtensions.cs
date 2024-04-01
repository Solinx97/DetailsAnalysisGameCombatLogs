using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Enums;
using CombatAnalysis.CommunicationDAL.Interfaces;
using CombatAnalysis.CommunicationDAL.Repositories.Firebase;
using CombatAnalysis.CommunicationDAL.Repositories.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CommunicationDAL.Extensions;

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

        services.AddDbContext<SQLContext>(options =>
        {
            options.UseSqlServer(connection);
        });

        services.AddScoped<IGenericRepository<Post, int>, SQLRepository<Post, int>>();
        services.AddScoped<IGenericRepository<PostDislike, int>, SQLRepository<PostDislike, int>>();
        services.AddScoped<IGenericRepository<PostLike, int>, SQLRepository<PostLike, int>>();
        services.AddScoped<IGenericRepository<PostComment, int>, SQLRepository<PostComment, int>>();
        services.AddScoped<IGenericRepository<UserPost, int>, SQLRepository<UserPost, int>>();
        services.AddScoped<IGenericRepository<Community, int>, SQLRepository<Community, int>>();
        services.AddScoped<IGenericRepository<CommunityDiscussion, int>, SQLRepository<CommunityDiscussion, int>>();
        services.AddScoped<IGenericRepository<CommunityDiscussionComment, int>, SQLRepository<CommunityDiscussionComment, int>>();
        services.AddScoped<IGenericRepository<CommunityPost, int>, SQLRepository<CommunityPost, int>>();
        services.AddScoped<IGenericRepository<CommunityUser, string>, SQLRepository<CommunityUser, string>>();
        services.AddScoped<IGenericRepository<InviteToCommunity, int>, SQLRepository<InviteToCommunity, int>>();
    }

    private static void FirebaseDatabase(IServiceCollection services)
    {
        services.AddDbContext<FirebaseContext>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(FirebaseRepository<,>));
    }
}
