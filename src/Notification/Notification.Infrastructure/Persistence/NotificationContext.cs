using Microsoft.EntityFrameworkCore;
using Notification.Infrastructure.Extensions;
using Notification.Infrastructure.Persistence.Outbox;

namespace Notification.Infrastructure.Persistence;

public class NotificationContext(DbContextOptions<NotificationContext> options) : DbContext(options)
{
    public DbSet<Domain.Aggregates.Notification> Notification { get; set; } = null!;

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Creating();

        base.OnModelCreating(modelBuilder);
    }
}
