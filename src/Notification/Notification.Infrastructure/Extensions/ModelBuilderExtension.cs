using Microsoft.EntityFrameworkCore;
using Notification.Domain.ValueObjects;

namespace Notification.Infrastructure.Extensions;

internal static class ModelBuilderExtension
{
    public static void Creating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Aggregates.Notification>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new NotificationId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.InitiatorId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(c => c.RecipientId).HasConversion(
                            id => id.Value,
                            value => new UserId(value)
                        );

            builder.Property(c => c.InitiatorName).HasMaxLength(Domain.Aggregates.Notification.NAME_MAX_LENGTH);
        });
    }
}
