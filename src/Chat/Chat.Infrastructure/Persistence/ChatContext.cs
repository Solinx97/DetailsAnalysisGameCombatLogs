using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Infrastructure.Extensions;
using Chat.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence;

public class ChatContext(DbContextOptions<ChatContext> options) : DbContext(options)
{
    public DbSet<VoiceChat> VoiceChat => Set<VoiceChat>();

    public DbSet<VoiceChatParticipant> VoiceChatParticipant => Set<VoiceChatParticipant>();

    public DbSet<PersonalChat> PersonalChat => Set<PersonalChat>();

    public DbSet<PersonalChatMessage> PersonalChatMessage => Set<PersonalChatMessage>();

    public DbSet<GroupChat> GroupChat => Set<GroupChat>();

    public DbSet<GroupChatMessage> GroupChatMessage => Set<GroupChatMessage>();

    public DbSet<GroupChatUser> GroupChatUser => Set<GroupChatUser>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Creating();
    }
}
