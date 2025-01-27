using CombatAnalysis.UserDAL.Data;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Enums;
using CombatAnalysis.UserDAL.Interfaces;
using CombatAnalysis.UserDAL.Repositories.Firebase;
using CombatAnalysis.UserDAL.Repositories.SQL;
using CombatAnalysis.UserDAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.UserDAL.Extensions;

public static class DataCollectionExtensions
{
    public static void UserDALDependencies(this IServiceCollection services, string databaseName, string dataProcessingType, string connectionString)
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
        services.AddDbContext<UserSQLContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IUserRepository, SQLUserRepository>();
        services.AddScoped<IGenericRepository<Customer, string>, SQLRepository<Customer, string>>();
        services.AddScoped<IGenericRepository<Friend, int>, SQLRepository<Friend, int>>();
        services.AddScoped<IGenericRepository<BannedUser, int>, SQLRepository<BannedUser, int>>();
        services.AddScoped<IGenericRepository<RequestToConnect, int>, SQLRepository<RequestToConnect, int>>();
    }

    private static void FirebaseDatabase(IServiceCollection services)
    {
        services.AddDbContext<FirebaseContext>();

        services.AddScoped<IUserRepository, FIrebaseUserRepository>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(FirebaseRepository<,>));
    }
}
