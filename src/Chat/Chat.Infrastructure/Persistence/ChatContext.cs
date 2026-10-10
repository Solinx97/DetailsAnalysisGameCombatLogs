using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Infrastructure.Extensions;
using Chat.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence;

public class ChatContext(DbContextOptions<ChatContext> options) : DbContext(options)
{
    public DbSet<VoiceChat> VoiceChat { get; set; } = null!;

    public DbSet<VoiceChatParticipant> VoiceChatParticipant { get; set; } = null!;

    public DbSet<PersonalChat> PersonalChat { get; set; } = null!;

    public DbSet<PersonalChatMessage> PersonalChatMessage { get; set; } = null!;

    public DbSet<GroupChat> GroupChat { get; set; } = null!;

    public DbSet<GroupChatMessage> GroupChatMessage { get; set; } = null!;

    public DbSet<GroupChatUser> GroupChatUser { get; set; } = null!;

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Creating();

        base.OnModelCreating(modelBuilder);
    }
}
