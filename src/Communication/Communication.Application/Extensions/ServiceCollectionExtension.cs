using Communication.Application.Commands.CreateCommunity;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddMediatorSource(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateCommunityCommand).Assembly));
    }
}
