using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Data;

public class ChatSQLContext : DbContext
{
    public ChatSQLContext(DbContextOptions<ChatSQLContext> options) : base(options)
    {
        var isExists = Database.EnsureCreated();

        if (isExists)
        {
            DbContextHelper.CreateProcedures(this);
        }
    }

    #region Chat

    public DbSet<VoiceChat>? VoiceChat { get; }

    public DbSet<PersonalChat>? PersonalChat { get; }

    public DbSet<PersonalChatMessage>? PersonalChatMessage { get; }

    public DbSet<PersonalChatMessageCount>? PersonalChatMessageCount { get; }

    public DbSet<GroupChat>? GroupChat { get; }

    public DbSet<GroupChatRules>? GroupChatRules { get; }

    public DbSet<GroupChatMessage>? GroupChatMessage { get; }

    public DbSet<UnreadGroupChatMessage>? UnreadGroupChatMessage { get; }

    public DbSet<GroupChatMessageCount>? GroupChatMessageCount { get; }

    public DbSet<GroupChatUser>? GroupChatUser { get; }

    #endregion
}
