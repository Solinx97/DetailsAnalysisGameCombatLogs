using CombatAnalysis.DAL.Consts;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data;

public class CombatParserSQLContext : DbContext
{
    public CombatParserSQLContext(DbContextOptions<CombatParserSQLContext> options) : base(options)
    {
        var isExists = Database.EnsureCreated();
        Database.SetCommandTimeout(DBConfigurations.CommandTimeout);

        if (isExists)
        {
            DbContextHelper.CreateProcedures(this);
        }
    }

    #region Combat

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<CombatLogByUser>? CombatLogByUser { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<CombatAura>? CombatAura { get; }

    public DbSet<CombatPlayer>? CombatPlayer { get; }

    public DbSet<CombatPlayerPosition>? CombatPlayerPosition { get; }

    public DbSet<PlayerParseInfo>? PlayerParseInfo { get; }

    public DbSet<SpecializationScore>? SpecializationScore { get; }

    public DbSet<DamageDone>? DamageDone { get; }

    public DbSet<DamageDoneGeneral>? DamageDoneGeneral { get; }

    public DbSet<HealDone>? HealDone { get; }

    public DbSet<HealDoneGeneral>? HealDoneGeneral { get; }

    public DbSet<DamageTaken>? DamageTaken { get; }

    public DbSet<DamageTakenGeneral>? DamageTakenGeneral { get; }

    public DbSet<ResourceRecovery>? ResourceRecovery { get; }

    public DbSet<ResourceRecoveryGeneral>? ResourceRecoveryGeneral { get; }

    public DbSet<PlayerDeath>? PlayerDeath { get; }

    #endregion
}