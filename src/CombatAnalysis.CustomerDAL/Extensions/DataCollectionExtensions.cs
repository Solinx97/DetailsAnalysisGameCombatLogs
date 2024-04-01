using CombatAnalysis.CustomerDAL.Data;
using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Enums;
using CombatAnalysis.CustomerDAL.Interfaces;
using CombatAnalysis.CustomerDAL.Repositories.Firebase;
using CombatAnalysis.CustomerDAL.Repositories.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CustomerDAL.Extensions;

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
