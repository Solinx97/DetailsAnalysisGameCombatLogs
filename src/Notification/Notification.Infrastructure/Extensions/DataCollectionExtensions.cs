using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces.Persistence;
using Notification.Domain.Repositories;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Persistence.Outbox;
using Notification.Infrastructure.Repositories;

namespace Notification.Infrastructure.Extensions;

public static class DataCollectionExtensions
{
    public static void AddNotificationInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<NotificationContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IOutboxRepository, OutboxRepository>();

        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
