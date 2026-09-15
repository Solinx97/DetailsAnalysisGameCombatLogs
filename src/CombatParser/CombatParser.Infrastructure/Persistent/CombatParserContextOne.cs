using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Entities.WoWMidnight;
using CombatParser.Domain.Entities.WoWMoPClassic;
using CombatParser.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Persistent;

public class CombatParserContextOne(DbContextOptions<CombatParserContextOne> options) : DbContext(options)
{
    public DbSet<Player>? Player { get; }

    public DbSet<Boss>? Boss { get; }

    public DbSet<BossMap>? BossMap { get; }

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<CombatAbility>? CombatAbility { get; }

    public DbSet<CombatPlayer>? CombatPlayer { get; }

    public DbSet<Unit>? Unit { get; }

    public DbSet<UnitInfo>? UnitInfo { get; }

    public DbSet<UnitHealth>? UnitHealth { get; }

    public DbSet<UnitCast>? UnitCast { get; }

    public DbSet<UnitPosition>? UnitPosition { get; }

    public DbSet<UnitPreAura>? UnitPreAura { get; }

    public DbSet<UnitAura>? UnitAura { get; }

    public DbSet<DamageDone>? DamageDone { get; }

    public DbSet<DamageDoneGeneral>? DamageDoneGeneral { get; }

    public DbSet<HealDone>? HealDone { get; }

    public DbSet<HealDoneGeneral>? HealDoneGeneral { get; }

    public DbSet<ResourceRecovery>? ResourceRecovery { get; }

    public DbSet<ResourceRecoveryGeneral>? ResourceRecoveryGeneral { get; }

    public DbSet<WoWMoPClassicPlayerStats>? WoWMoPClassicPlayerStats { get; }

    public DbSet<WoWMidnightPlayerStats>? WoWMidnightPlayerStats { get; }

    public DbSet<Specialization>? Specialization { get; }

    public DbSet<SpecializationScore>? SpecializationScore { get; }

    public DbSet<BestSpecializationScore>? BestSpecializationScore { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Creating();
    }
}