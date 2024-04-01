using CombatAnalysis.ChatDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Data;

public class SQLContext : DbContext
{
    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    #region Chat

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
