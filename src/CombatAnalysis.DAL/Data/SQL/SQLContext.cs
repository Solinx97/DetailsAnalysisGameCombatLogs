using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data.SQL;

public class SQLContext : DbContext
{
    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
        var isExists = Database.EnsureCreated();
        if (isExists)
        {
            DbProcedureHelper.CreateProcedures(this);
        }
    }

    public DbSet<AppUser>? AppUser { get; }

    public DbSet<RefreshToken>? RefreshToken { get; }

    public DbSet<PersonalChat>? PersonalChat { get; }

    public DbSet<PersonalChatMessage>? PersonalChatMessage { get; }

    public DbSet<InviteToGroupChat>? InviteToGroupChat { get; }

    public DbSet<GroupChat>? GroupChat { get; }

    public DbSet<GroupChatMessage>? GroupChatMessage { get; }

    public DbSet<GroupChatUser>? GroupChatUser { get; }

    public DbSet<BannedUser>? BannedUser { get; }

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<CombatLogByUser>? CombatLogByUser { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<CombatPlayer>? CombatPlayer { get; }

    public DbSet<DamageDone>? DamageDone { get; }

    public DbSet<DamageDoneGeneral>? DamageDoneGeneral { get; }

    public DbSet<HealDone>? HealDone { get; }

    public DbSet<HealDoneGeneral>? HealDoneGeneral { get; }

    public DbSet<DamageTaken>? DamageTaken { get; }

    public DbSet<DamageTakenGeneral>? DamageTakenGeneral { get; }

    public DbSet<ResourceRecovery>? ResourceRecovery { get; }

    public DbSet<ResourceRecoveryGeneral>? ResourceRecoveryGeneral { get; }
}