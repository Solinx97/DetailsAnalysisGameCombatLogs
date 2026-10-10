using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Extensions;

internal static class ModelBuilderExtension
{
    public static void Creating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroupChat>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new GroupChatId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.OwnerId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

            builder.OwnsOne(c => c.Rules, r =>
            {
                r.HasKey(c => c.Id);

                r.WithOwner().HasForeignKey(m => m.GroupChatId);

                r.Property(x => x.Id)
                     .HasConversion(
                         id => id.Value,
                         value => new GroupChatRulesId(value)
                     ).ValueGeneratedOnAdd();

                r.Property(x => x.GroupChatId)
                    .HasConversion(
                        id => id.Value,
                        value => new GroupChatId(value)
                    );

                r.ToTable(nameof(GroupChatRules));
            });

            builder.HasMany(c => c.Messages)
                   .WithOne(m => m.GroupChat)
                   .HasForeignKey(m => m.GroupChatId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Users)
                   .WithOne(m => m.GroupChat)
                   .HasForeignKey(m => m.GroupChatId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GroupChatMessage>(builder =>
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Username).IsRequired().HasMaxLength(GroupChatMessage.USERNAME_MAX_LENGTH);
            builder.Property(m => m.Message).IsRequired().HasMaxLength(GroupChatMessage.MESSAGE_MAX_LENGTH);

            builder.Property(m => m.Time).IsRequired();

            builder.Property(c => c.Time)
                   .HasConversion(
                       dto => dto.UtcDateTime,
                       dt => new DateTimeOffset(dt)
                   );

            builder.Property(x => x.Id)
                     .HasConversion(
                         id => id.Value,
                         value => new GroupChatMessageId(value)
                     ).ValueGeneratedOnAdd();

            builder.Property(x => x.GroupChatUserId)
                     .HasConversion(
                         id => id.Value,
                         value => new GroupChatUserId(value)
                     );
        });

        modelBuilder.Entity<GroupChatUser>(builder =>
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Username).IsRequired().HasMaxLength(Domain.Entities.GroupChatUser.USERNAME_MAX_LENGTH);

            builder.Property(x => x.Id)
                 .HasConversion(
                     id => id.Value,
                     value => new GroupChatUserId(value)
                 );

            builder.Property(x => x.LastReadMessageId)
                 .HasConversion(
                     id => id != null ? id.Value : Guid.NewGuid(),
                     value => new GroupChatMessageId(value)
                 );

            builder.Property(x => x.AppUserId)
                 .HasConversion(
                     id => id.Value,
                     value => new UserId(value)
                 );
        });

        modelBuilder.Entity<PersonalChat>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new PersonalChatId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.InitiatorId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(c => c.CompanionId).HasConversion(
                            id => id.Value,
                            value => new UserId(value)
                        );

            builder.HasMany(c => c.Messages)
                   .WithOne(m => m.PersonalChat)
                   .HasForeignKey(m => m.PersonalChatId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PersonalChatMessage>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(msg => msg.Username).IsRequired().HasMaxLength(PersonalChatMessage.USERNAME_MAX_LENGTH);
            builder.Property(msg => msg.Message).IsRequired().HasMaxLength(PersonalChatMessage.MESSAGE_MAX_LENGTH);

            builder.Property(m => m.Time).IsRequired();

            builder.Property(c => c.Time)
                   .HasConversion(
                       dto => dto.UtcDateTime,
                       dt => new DateTimeOffset(dt)
                   );

            builder.Property(x => x.Id)
                     .HasConversion(
                         id => id.Value,
                         value => new PersonalChatMessageId(value)
                     ).ValueGeneratedOnAdd();

            builder.Property(x => x.AppUserId)
                     .HasConversion(
                         id => id.Value,
                         value => new UserId(value)
                     );
        });

        modelBuilder.Entity<VoiceChat>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new VoiceChatId(value)
                       );

            builder.Property(c => c.GroupChatId).HasConversion(
                           id => id.Value,
                           value => new GroupChatId(value)
                       );

            builder.HasMany(c => c.Participants)
                   .WithOne(m => m.VoiceChat)
                   .HasForeignKey(m => m.VoiceChatId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VoiceChatParticipant>(builder =>
        {
            builder.HasKey(m => m.Id);

            builder.Property(x => x.Id)
                 .HasConversion(
                     id => id.Value,
                     value => new VoiceChatParticipantId(value)
                 );
            builder.Property(x => x.VoiceChatId)
                 .HasConversion(
                     id => id.Value,
                     value => new VoiceChatId(value)
                 );

            builder.Property(x => x.AppUserId)
                 .HasConversion(
                     id => id.Value,
                     value => new UserId(value)
                 );
        });
    }
}
