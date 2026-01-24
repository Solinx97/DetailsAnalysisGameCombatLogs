using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Persistence;

public class CombatParserContext(DbContextOptions<CombatParserContext> options) : DbContext(options)
{
    public DbSet<Player>? Player { get; }

    public DbSet<Boss>? Boss { get; }

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<CombatAura>? CombatAura { get; }

    public DbSet<CombatPlayer>? CombatPlayer { get; }

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
        modelBuilder.Entity<Boss>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Boss>().HasData(MigrationBuilderExtension.GenerateBossCollection());

        modelBuilder.Entity<Specialization>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Specialization>().HasData(MigrationBuilderExtension.GenerateSpecializationCollection());

        modelBuilder.Entity<BestSpecializationScore>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<BestSpecializationScore>().HasData(MigrationBuilderExtension.GenerateBestSpecializationScoreCollection());

        AddTableRefs(modelBuilder);
    }

    private static void AddTableRefs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Combat>()
            .HasOne(c => c.CombatLog)
            .WithMany(cl => cl.Combats)
            .HasForeignKey(c => c.CombatLogId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CombatAura>()
            .HasOne(bs => bs.Combat)
            .WithMany(s => s.CombatAuras)
            .HasForeignKey(bs => bs.CombatId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CombatTarget>()
            .HasOne(bs => bs.Combat)
            .WithMany(s => s.CombatTargets)
            .HasForeignKey(bs => bs.CombatId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CombatPlayer>()
            .HasOne(cp => cp.Combat)
            .WithMany(c => c.CombatPlayers)
            .HasForeignKey(cp => cp.CombatId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CombatPlayer>()
            .HasOne(cp => cp.Stats)
            .WithOne(s => s.CombatPlayer)
            .HasForeignKey<CombatPlayerStats>(s => s.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CombatPlayer>()
            .HasOne(cp => cp.Score)
            .WithOne(s => s.CombatPlayer)
            .HasForeignKey<SpecializationScore>(s => s.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CombatPlayer>()
            .HasOne(cp => cp.Player)
            .WithMany(p => p.CombatPlayers)
            .HasForeignKey(cp => cp.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CombatPlayerDeath>()
            .HasOne(cpd => cpd.CombatPlayer)
            .WithMany(cp => cp.CombatPlayerDeathes)
            .HasForeignKey(cpd => cpd.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CombatPlayerPosition>()
            .HasOne(cpp => cpp.CombatPlayer)
            .WithMany(cp => cp.CombatPlayerPositions)
            .HasForeignKey(cpp => cpp.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CombatPlayerPosition>()
            .HasOne(cpp => cpp.Combat)
            .WithMany(cp => cp.CombatPlayerPositions)
            .HasForeignKey(cpp => cpp.CombatId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SpecializationScore>()
            .HasOne(sc => sc.Specialization)
            .WithMany(s => s.SpecializationScores)
            .HasForeignKey(sc => sc.SpecializationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BestSpecializationScore>()
            .HasOne(bss => bss.Specialization)
            .WithMany(s => s.BestSpecializationScores)
            .HasForeignKey(bss => bss.SpecializationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BestSpecializationScore>()
            .HasOne(bss => bss.Boss)
            .WithMany(b => b.BestSpecializationScores)
            .HasForeignKey(bss => bss.BossId)
            .OnDelete(DeleteBehavior.Restrict);

        AddCombatPlayerDataTableRefs(modelBuilder);
    }

    private static void AddCombatPlayerDataTableRefs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DamageDone>()
            .HasOne(ddg => ddg.CombatPlayer)
            .WithMany(cp => cp.DamageDones)
            .HasForeignKey(ddg => ddg.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DamageDoneGeneral>()
            .HasOne(ddg => ddg.CombatPlayer)
            .WithMany(cp => cp.DamageDoneGenerals)
            .HasForeignKey(ddg => ddg.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HealDone>()
            .HasOne(hd => hd.CombatPlayer)
            .WithMany(cp => cp.HealDones)
            .HasForeignKey(dn => dn.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HealDoneGeneral>()
            .HasOne(hdg => hdg.CombatPlayer)
            .WithMany(cp => cp.HealDoneGenerals)
            .HasForeignKey(hdg => hdg.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DamageTaken>()
            .HasOne(dt => dt.CombatPlayer)
            .WithMany(cp => cp.DamageTakens)
            .HasForeignKey(dt => dt.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DamageTakenGeneral>()
            .HasOne(dtg => dtg.CombatPlayer)
            .WithMany(cp => cp.DamageTakenGenerals)
            .HasForeignKey(dtg => dtg.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResourceRecovery>()
            .HasOne(rr => rr.CombatPlayer)
            .WithMany(cp => cp.ResourceRecoveries)
            .HasForeignKey(rr => rr.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResourceRecoveryGeneral>()
            .HasOne(rrg => rrg.CombatPlayer)
            .WithMany(cp => cp.ResourceRecoveryGenerals)
            .HasForeignKey(rrg => rrg.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}