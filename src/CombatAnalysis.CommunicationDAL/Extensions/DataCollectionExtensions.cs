﻿using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Enums;
using CombatAnalysis.CommunicationDAL.Interfaces;
using CombatAnalysis.CommunicationDAL.Repositories.Firebase;
using CombatAnalysis.CommunicationDAL.Repositories.SQL;
using CombatAnalysis.CommunicationDAL.Repositories.SQL.StoredProcedure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CommunicationDAL.Extensions;

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
        services.AddDbContext<CommunicationSQLContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ICommunityRepository, SQLCommunityRepository>();
        services.AddScoped<IGenericRepository<CommunityDiscussion, int>, SQLRepository<CommunityDiscussion, int>>();
        services.AddScoped<IGenericRepository<CommunityDiscussionComment, int>, SQLRepository<CommunityDiscussionComment, int>>();
        services.AddScoped<IGenericRepository<CommunityUser, string>, SQLRepository<CommunityUser, string>>();
        services.AddScoped<IGenericRepository<InviteToCommunity, int>, SQLRepository<InviteToCommunity, int>>();
        services.AddScoped<ICommunityPostRepository, SQLSPCommunityPostRepository>();
        services.AddScoped<IGenericRepository<CommunityPostComment, int>, SQLRepository<CommunityPostComment, int>>();
        services.AddScoped<IGenericRepository<CommunityPostLike, int>, SQLRepository<CommunityPostLike, int>>();
        services.AddScoped<IGenericRepository<CommunityPostDislike, int>, SQLRepository<CommunityPostDislike, int>>();
        services.AddScoped<IUserPostRepository, SQLSPUserPostRepository>();
        services.AddScoped<IGenericRepository<UserPostComment, int>, SQLRepository<UserPostComment, int>>();
        services.AddScoped<IGenericRepository<UserPostLike, int>, SQLRepository<UserPostLike, int>>();
        services.AddScoped<IGenericRepository<UserPostDislike, int>, SQLRepository<UserPostDislike, int>>();
    }

    private static void FirebaseDatabase(IServiceCollection services)
    {
        services.AddDbContext<FirebaseContext>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(FirebaseRepository<,>));
    }
}
