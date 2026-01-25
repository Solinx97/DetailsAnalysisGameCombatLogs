using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Data;
using CombatParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatParser.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CombatParserContextOne>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IGenericRepository<CombatLog, int>, GenericRepository<CombatLog, int>>();
        services.AddScoped<IGenericRepository<Combat, int>, GenericRepository<Combat, int>>();
        services.AddScoped<ICombatLogRepository, CombatLogRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
