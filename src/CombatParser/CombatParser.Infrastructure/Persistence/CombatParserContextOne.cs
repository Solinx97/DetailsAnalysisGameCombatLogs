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

        modelBuilder.Entity<Combat>(b =>
        {
            b.HasKey(c => c.Id);

            b.OwnsMany(c => c.CombatPlayers, cp =>
            {
                cp.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayer.CombatId));
                cp.Property<int>(nameof(Domain.Entities.CombatPlayer.Id));
                cp.HasKey(nameof(Domain.Entities.CombatPlayer.Id));

                cp.OwnsOne(p => p.Stats, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerStats.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerStats.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerStats.Id));
                });

                cp.OwnsOne(p => p.Score, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.SpecializationScore.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.SpecializationScore.Id));
                    dd.HasKey(nameof(Domain.Entities.SpecializationScore.Id));
                });

                cp.OwnsMany(p => p.DamageDones, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerData.DamageDone.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerData.DamageDone.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerData.DamageDone.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.DamageDone.Spell))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.DamageDone.SPELL_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.DamageDone.Creator))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.DamageDone.CREATOR_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.DamageDone.Target))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.DamageDone.TARGET_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.DamageDoneGenerals, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerData.DamageDoneGeneral.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerData.DamageDoneGeneral.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerData.DamageDoneGeneral.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.DamageDoneGeneral.Spell))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.DamageDoneGeneral.SPELL_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.HealDones, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerData.HealDone.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerData.HealDone.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerData.HealDone.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.HealDone.Spell))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.HealDone.SPELL_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.HealDone.Creator))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.HealDone.CREATOR_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.HealDone.Target))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.HealDone.TARGET_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.HealDoneGenerals, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerData.HealDoneGeneral.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerData.HealDoneGeneral.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerData.HealDoneGeneral.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.HealDoneGeneral.Spell))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.HealDoneGeneral.SPELL_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.DamageTakens, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerData.DamageTaken.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerData.DamageTaken.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerData.DamageTaken.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.DamageTaken.Spell))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.DamageTaken.SPELL_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.DamageTaken.Creator))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.DamageTaken.CREATOR_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.DamageTaken.Target))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.DamageTaken.TARGET_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.DamageTakenGenerals, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerData.DamageTakenGeneral.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerData.DamageTakenGeneral.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerData.DamageTakenGeneral.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.DamageTakenGeneral.Spell))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.DamageTakenGeneral.SPELL_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.ResourceRecoveries, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerData.ResourceRecovery.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerData.ResourceRecovery.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerData.ResourceRecovery.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.ResourceRecovery.Spell))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.ResourceRecovery.SPELL_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.ResourceRecovery.Creator))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.ResourceRecovery.CREATOR_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.ResourceRecovery.Target))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.ResourceRecovery.TARGET_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.ResourceRecoveryGenerals, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerData.ResourceRecoveryGeneral.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerData.ResourceRecoveryGeneral.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerData.ResourceRecoveryGeneral.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerData.ResourceRecoveryGeneral.Spell))
                        .HasMaxLength(Domain.Entities.CombatPlayerData.ResourceRecoveryGeneral.SPELL_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.CombatPlayerDeathes, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerDeath.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerDeath.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerDeath.Id));

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerDeath.Username))
                        .HasMaxLength(Domain.Entities.CombatPlayerDeath.USERNAME_MAX_LENGTH);

                    dd.Property<string>(nameof(Domain.Entities.CombatPlayerDeath.LastHitSpell))
                        .HasMaxLength(Domain.Entities.CombatPlayerDeath.SPELL_MAX_LENGTH);
                });

                cp.OwnsMany(p => p.CombatPlayerPositions, dd =>
                {
                    dd.WithOwner().HasForeignKey(nameof(Domain.Entities.CombatPlayerPosition.CombatPlayerId));
                    dd.Property<int>(nameof(Domain.Entities.CombatPlayerPosition.Id));
                    dd.HasKey(nameof(Domain.Entities.CombatPlayerPosition.Id));
                });
            });
        });
    }
}