using CombatParser.Application.Commands.CreateCombatLog;
using Microsoft.Extensions.DependencyInjection;

namespace CombatParser.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddMediatorSource(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateCombatLogCommand).Assembly));
    }
}
