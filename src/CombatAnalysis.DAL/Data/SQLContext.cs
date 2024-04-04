using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data;

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

    #region Combat details

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<CombatLogByUser>? CombatLogByUser { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<CombatPlayer>? CombatPlayer { get; }

    public DbSet<PlayerParseInfo>? PlayerParseInfo { get; }

    public DbSet<DamageDone>? DamageDone { get; }

    public DbSet<DamageDoneGeneral>? DamageDoneGeneral { get; }

    public DbSet<HealDone>? HealDone { get; }

    public DbSet<HealDoneGeneral>? HealDoneGeneral { get; }

    public DbSet<DamageTaken>? DamageTaken { get; }

    public DbSet<DamageTakenGeneral>? DamageTakenGeneral { get; }

    public DbSet<ResourceRecovery>? ResourceRecovery { get; }

    public DbSet<ResourceRecoveryGeneral>? ResourceRecoveryGeneral { get; }

    #endregion
}