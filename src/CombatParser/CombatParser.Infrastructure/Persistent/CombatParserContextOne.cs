using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Persistent;

public class CombatParserContextOne(DbContextOptions<CombatParserContextOne> options) : DbContext(options)
{
    public DbSet<Player>? Player { get; }

    public DbSet<Boss>? Boss { get; }

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<BossMap>? BossMap { get; }

    public DbSet<CombatPlayerPreAura>? CombatPlayerPreAura { get; }

    public DbSet<CombatPlayerAura>? CombatPlayerAura { get; }

    public DbSet<CombatAbility>? CombatAbility { get; }

    public DbSet<CombatPlayer>? CombatPlayer { get; }

    public DbSet<CombatTarget>? CombatTarget { get; }

    public DbSet<CombatPlayerPosition>? CombatPlayerPosition { get; }

    public DbSet<DamageDone>? DamageDone { get; }

    public DbSet<DamageDoneGeneral>? DamageDoneGeneral { get; }

    public DbSet<HealDone>? HealDone { get; }

    public DbSet<HealDoneGeneral>? HealDoneGeneral { get; }

    public DbSet<DamageTaken>? DamageTaken { get; }

    public DbSet<DamageTakenGeneral>? DamageTakenGeneral { get; }

    public DbSet<ResourceRecovery>? ResourceRecovery { get; }

    public DbSet<ResourceRecoveryGeneral>? ResourceRecoveryGeneral { get; }

    public DbSet<CombatPlayerDeath>? CombatPlayerDeath { get; }

    public DbSet<CombatPlayerStats>? CombatPlayerStats { get; }

    public DbSet<Specialization>? Specialization { get; }

    public DbSet<SpecializationScore>? SpecializationScore { get; }

    public DbSet<BestSpecializationScore>? BestSpecializationScore { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Creating();
    }
}