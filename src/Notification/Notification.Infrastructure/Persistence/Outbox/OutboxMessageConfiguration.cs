using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.Infrastructure.Persistence.Outbox;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Topic)
            .HasMaxLength(OutboxMessage.TOPIC_MAX_LENGTH)
            .IsRequired();

        builder.Property(x => x.Key)
            .HasMaxLength(OutboxMessage.KEY_MAX_LENGTH)
            .IsRequired();

        builder.Property(x => x.Payload)
            .IsRequired();

        builder.HasIndex(x => x.ProcessedAt);
        builder.HasIndex(x => x.CreatedAt);
    }
}
