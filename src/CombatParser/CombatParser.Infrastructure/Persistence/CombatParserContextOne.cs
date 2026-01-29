using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Persistence;

public class CombatParserContextOne(DbContextOptions<CombatParserContextOne> options) : DbContext(options)
{
    public DbSet<Player>? Player { get; }

    public DbSet<Boss>? Boss { get; }

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<CombatAura>? CombatAura { get; }

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

        modelBuilder.Entity<CombatPlayer>()
            .HasOne(ddg => ddg.Combat)
            .WithMany(cp => cp.CombatPlayers)
            .HasForeignKey(ddg => ddg.CombatId)
            .OnDelete(DeleteBehavior.Cascade);

        AddTableRefs(modelBuilder);
    }

    private static void AddTableRefs(ModelBuilder modelBuilder)
    {
        // CombatLog
        modelBuilder.Entity<CombatLog>(cl =>
        {
            cl.Property(p => p.Name)
                .HasMaxLength(Domain.Aggregates.CombatLog.NAME_MAX_LENGTH);
        });

        // Combat
        modelBuilder.Entity<Combat>(c =>
        {
            c.Property(p => p.DungeonName)
                .HasMaxLength(Domain.Aggregates.Combat.DUNGEON_NAME_MAX_LENGTH);

            c.HasOne(p => p.CombatLog)
                .WithMany(cl => cl.Combats)
                .HasForeignKey(p => p.CombatLogId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CombatAura>(ca =>
        {
            ca.Property(p => p.Name)
                .HasMaxLength(Domain.Entities.CombatAura.NAME_MAX_LENGTH);

            ca.Property(p => p.Creator)
                .HasMaxLength(Domain.Entities.CombatAura.CREATOR_MAX_LENGTH);
            
            ca.Property(p => p.Target)
                .HasMaxLength(Domain.Entities.CombatAura.TARGET_MAX_LENGTH);

            ca.HasOne(bs => bs.Combat)
                .WithMany(s => s.CombatAuras)
                .HasForeignKey(bs => bs.CombatId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CombatTarget>(ct =>
        {
            ct.Property(p => p.Username)
                .HasMaxLength(Domain.Entities.CombatTarget.USERNAME_MAX_LENGTH);

            ct.Property(p => p.Target)
                .HasMaxLength(Domain.Entities.CombatTarget.TARGET_MAX_LENGTH);

            ct.HasOne(bs => bs.Combat)
                .WithMany(s => s.CombatTargets)
                .HasForeignKey(bs => bs.CombatId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CombatPlayerStats>(cps =>
        {
            cps.Property(p => p.Talents)
                .HasMaxLength(Domain.Entities.CombatPlayerData.CombatPlayerStats.TALENTS_MAX_LENGTH);

            cps.HasOne(ddg => ddg.CombatPlayer)
                .WithOne(cp => cp.Stats)
                .HasForeignKey<CombatPlayerStats>(s => s.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SpecializationScore>(ss =>
        {
            ss.HasOne(ss => ss.CombatPlayer)
                .WithOne(cp => cp.Score)
                .HasForeignKey<SpecializationScore>(s => s.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            ss.HasOne(sc => sc.Specialization)
                .WithMany(s => s.SpecializationScores)
                .HasForeignKey(sc => sc.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CombatPlayer>(cp =>
        {
            cp.HasOne(cp => cp.Combat)
                .WithMany(c => c.CombatPlayers)
                .HasForeignKey(cp => cp.CombatId)
                .OnDelete(DeleteBehavior.Cascade);

            cp.HasOne(cp => cp.Player)
                .WithMany(p => p.CombatPlayers)
                .HasForeignKey(cp => cp.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BestSpecializationScore>(bss =>
        {
            bss.HasOne(bss => bss.Specialization)
                .WithMany(s => s.BestSpecializationScores)
                .HasForeignKey(bss => bss.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            bss.HasOne(bss => bss.Boss)
                .WithMany(b => b.BestSpecializationScores)
                .HasForeignKey(bss => bss.BossId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Player>(ct =>
        {
            ct.Property(p => p.GameId)
                .HasMaxLength(Domain.Entities.Player.GAMEID_MAX_LENGTH);

            ct.Property(p => p.Username)
                .HasMaxLength(Domain.Entities.Player.USERNAME_MAX_LENGTH);
        });

        modelBuilder.Entity<Specialization>(s =>
        {
            s.Property(p => p.Name)
                .HasMaxLength(Domain.Entities.Specialization.NAME_MAX_LENGTH);

            s.Property(p => p.SpecializationSpellsId)
                .HasMaxLength(Domain.Entities.Specialization.SPEC_SPELLS_MAX_LENGTH);
        });

        modelBuilder.Entity<Boss>(b =>
        {
            b.Property(p => p.Name)
                .HasMaxLength(Domain.Entities.Boss.NAME_MAX_LENGTH);
        });

        AddCombatPlayerDataTableRefs(modelBuilder);
    }

    private static void AddCombatPlayerDataTableRefs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DamageDone>(dd =>
        {
            dd.Property(p => p.Spell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.DamageDone.SPELL_MAX_LENGTH);

            dd.Property(p => p.Creator)
                .HasMaxLength(Domain.Entities.CombatPlayerData.DamageDone.CREATOR_MAX_LENGTH);

            dd.Property(p => p.Target)
                .HasMaxLength(Domain.Entities.CombatPlayerData.DamageDone.TARGET_MAX_LENGTH);

            dd.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.DamageDones)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DamageDoneGeneral>(ddg =>
        {
            ddg.Property(p => p.Spell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.DamageDoneGeneral.SPELL_MAX_LENGTH);

            ddg.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.DamageDoneGenerals)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HealDone>(hd =>
        {
            hd.Property(p => p.Spell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.HealDone.SPELL_MAX_LENGTH);

            hd.Property(p => p.Creator)
                .HasMaxLength(Domain.Entities.CombatPlayerData.HealDone.CREATOR_MAX_LENGTH);

            hd.Property(p => p.Target)
                .HasMaxLength(Domain.Entities.CombatPlayerData.HealDone.TARGET_MAX_LENGTH);

            hd.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.HealDones)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HealDoneGeneral>(hdg =>
        {
            hdg.Property(p => p.Spell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.HealDoneGeneral.SPELL_MAX_LENGTH);

            hdg.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.HealDoneGenerals)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DamageTaken>(dt =>
        {
            dt.Property(p => p.Spell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.DamageTaken.SPELL_MAX_LENGTH);

            dt.Property(p => p.Creator)
                .HasMaxLength(Domain.Entities.CombatPlayerData.DamageTaken.CREATOR_MAX_LENGTH);

            dt.Property(p => p.Target)
                .HasMaxLength(Domain.Entities.CombatPlayerData.DamageTaken.TARGET_MAX_LENGTH);

            dt.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.DamageTakens)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DamageTakenGeneral>(dtg =>
        {
            dtg.Property(p => p.Spell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.DamageTakenGeneral.SPELL_MAX_LENGTH);

            dtg.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.DamageTakenGenerals)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ResourceRecovery>(rr =>
        {
            rr.Property(p => p.Spell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.ResourceRecovery.SPELL_MAX_LENGTH);

            rr.Property(p => p.Creator)
                .HasMaxLength(Domain.Entities.CombatPlayerData.ResourceRecovery.CREATOR_MAX_LENGTH);

            rr.Property(p => p.Target)
                .HasMaxLength(Domain.Entities.CombatPlayerData.ResourceRecovery.TARGET_MAX_LENGTH);

            rr.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.ResourceRecoveries)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ResourceRecoveryGeneral>(rrg =>
        {
            rrg.Property(p => p.Spell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.ResourceRecoveryGeneral.SPELL_MAX_LENGTH);

            rrg.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.ResourceRecoveryGenerals)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CombatPlayerDeath>(cpd =>
        {
            cpd.Property(p => p.Username)
                .HasMaxLength(Domain.Entities.CombatPlayerData.CombatPlayerDeath.USERNAME_MAX_LENGTH);

            cpd.Property(p => p.LastHitSpell)
                .HasMaxLength(Domain.Entities.CombatPlayerData.CombatPlayerDeath.SPELL_MAX_LENGTH);

            cpd.HasOne(cpd => cpd.CombatPlayer)
                .WithMany(cp => cp.CombatPlayerDeathes)
                .HasForeignKey(cpd => cpd.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CombatPlayerPosition>()
            .HasOne(cpp => cpp.CombatPlayer)
            .WithMany(cp => cp.CombatPlayerPositions)
            .HasForeignKey(ddg => ddg.CombatPlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}