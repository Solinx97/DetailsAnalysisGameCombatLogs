using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Commands.CreateNotification;

namespace Notification.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddNotificationApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateNotificationCommand).Assembly));
    }
}
