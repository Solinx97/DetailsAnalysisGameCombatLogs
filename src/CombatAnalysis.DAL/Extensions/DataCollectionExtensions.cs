using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Data.SQL;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Repositories.Firebase;
using CombatAnalysis.DAL.Repositories.SQL;
using CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

        var dataProcessingType = configuration.GetSection("Database:DataProcessingType").Value??string.Empty;
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

        services.AddScoped(typeof(IGenericRepository<,>), typeof(FirebaseRepositroy<,>));
    }

    public static void MSSQLDAL(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, SQLUserRepository>();
        services.AddScoped<ITokenRepository, SQLTokenRepository>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(SQLRepository<,>));
    }

    public static void MSSQLStoredProcedureDAL(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, SQLSPUserRepository>();
        services.AddScoped<ITokenRepository, SQLSPTokenRepository>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(SQLSPRepository<,>));
    }
}
