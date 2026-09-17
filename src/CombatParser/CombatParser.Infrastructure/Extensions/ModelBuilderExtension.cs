using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Entities.WoWMidnight;
using CombatParser.Domain.Entities.WoWMoPClassic;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Extensions;

internal static class ModelBuilderExtension
{
    public static void Creating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BossMap>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<BossMap>().HasData(MigrationBuilderExtension.GenerateMaps());

        modelBuilder.Entity<Boss>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Boss>().HasData(MigrationBuilderExtension.GenerateBosses());

        modelBuilder.Entity<BestSpecializationScore>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<BestSpecializationScore>().HasData(MigrationBuilderExtension.GenerateBestSpecializationScores());

        modelBuilder.Entity<Specialization>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Specialization>().HasData(MigrationBuilderExtension.GenerateSpecializations());

        modelBuilder.Entity<CombatAbility>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<CombatAbility>().HasData(MigrationBuilderExtension.GenerateCombatAbilities());

        modelBuilder.Entity<CombatPlayer>()
            .HasOne(cp => cp.Combat)
            .WithMany(c => c.CombatPlayers)
            .HasForeignKey(cp => cp.CombatId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Unit>(u =>
        {
            u.Property(p => p.GameId)
                .HasMaxLength(Unit.GAMEID_MAX_LENGTH);

            u.HasOne(u => u.Combat)
                .WithMany(c => c.Units)
                .HasForeignKey(u => u.CombatId)
                .OnDelete(DeleteBehavior.Cascade);

            u.HasOne(x => x.UnitInfo)
                .WithOne()
                .HasForeignKey<UnitInfo>(x => x.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        AddTableRefs(modelBuilder);
    }

    private static void AddTableRefs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CombatLog>(cl =>
        {
            cl.Property(p => p.Name)
                .HasMaxLength(CombatLog.NAME_MAX_LENGTH);
        });

        modelBuilder.Entity<CombatLogStatus>(c =>
        {
            c.HasOne(p => p.CombatLog)
                .WithMany(cl => cl.Statuses)
                .HasForeignKey(p => p.CombatLogId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Combat>(c =>
        {
            c.Property(p => p.DungeonName)
                .HasMaxLength(Combat.DUNGEON_NAME_MAX_LENGTH);

            c.HasOne(p => p.CombatLog)
                .WithMany(cl => cl.Combats)
                .HasForeignKey(p => p.CombatLogId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BossMap>(bm =>
        {
            bm.Property(p => p.Name)
                .HasMaxLength(BossMap.NAME_MAX_LENGTH);
        });

        modelBuilder.Entity<Boss>(b =>
        {
            b.Property(p => p.Name)
                .HasMaxLength(Boss.NAME_MAX_LENGTH);

            b.HasOne(bss => bss.BossMap)
                .WithMany(b => b.Bosses)
                .HasForeignKey(p => p.BossMapId)
                .OnDelete(DeleteBehavior.Cascade);
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
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WoWMoPClassicPlayerStats>(cps =>
        {
            cps.Property(p => p.Talents)
                .HasMaxLength(WoWMoPClassicPlayerStats.TALENTS_MAX_LENGTH);

            cps.HasOne(cps => cps.CombatPlayer)
                .WithOne()
                .HasForeignKey<WoWMoPClassicPlayerStats>(p => p.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WoWMidnightPlayerStats>(cps =>
        {
            cps.Property(p => p.Talents)
                .HasMaxLength(WoWMidnightPlayerStats.TALENTS_MAX_LENGTH);

            cps.HasOne(cps => cps.CombatPlayer)
                .WithOne()
                .HasForeignKey<WoWMidnightPlayerStats>(p => p.CombatPlayerId)
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

            cp.HasOne(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Player>(ct =>
        {
            ct.Property(p => p.GameId)
                .HasMaxLength(Player.GAMEID_MAX_LENGTH);

            ct.Property(p => p.Username)
                .HasMaxLength(Player.USERNAME_MAX_LENGTH);
        });

        modelBuilder.Entity<Specialization>(s =>
        {
            s.Property(p => p.Name)
                .HasMaxLength(Specialization.NAME_MAX_LENGTH);

            s.Property(p => p.SpecializationSpellsId)
                .HasMaxLength(Specialization.SPEC_SPELLS_MAX_LENGTH);
        });

        AddUnitDataTableRefs(modelBuilder);
    }

    private static void AddUnitDataTableRefs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UnitHealth>(uh =>
        {
            uh.Property(p => p.OwnerGameId)
                .HasMaxLength(UnitHealth.OWNER_GAMEID_MAX_LENGTH);

            uh.HasOne(uh => uh.Unit)
                .WithMany(c => c.UnitHealthes)
                .HasForeignKey(uh => uh.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UnitCast>(uc =>
        {
            uc.Property(p => p.Spell)
                .HasMaxLength(UnitCast.SPELL_MAX_LENGTH);

            uc.Property(p => p.GameSpellId)
                .HasMaxLength(UnitCast.GAME_SPELL_MAX_LENGTH);

            uc.Property(p => p.OwnerGameId)
                .HasMaxLength(UnitCast.OWNER_GAME_MAX_LENGTH);

            uc.Property(p => p.TargetGameId)
                .HasMaxLength(UnitCast.OWNER_GAME_MAX_LENGTH);

            uc.HasOne(uc => uc.Unit)
                .WithMany(c => c.UnitCasts)
                .HasForeignKey(uc => uc.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UnitPosition>(uh =>
        {
            uh.Property(uh => uh.OwnerGameId)
                .HasMaxLength(UnitPosition.OWNER_GAMEID_MAX_LENGTH);

            uh.HasOne(uh => uh.Unit)
                .WithMany(c => c.UnitPositions)
                .HasForeignKey(uh => uh.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UnitPreAura>(cpa =>
        {
            cpa.HasOne(a => a.Unit)
                .WithMany(cp => cp.PreAuras)
                .HasForeignKey(a => a.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            cpa.HasOne(dd => dd.Target)
                .WithMany()
                .HasForeignKey(ddg => ddg.TargetId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<UnitAura>(a =>
        {
            a.Property(x => x.Name)
                .HasMaxLength(UnitAura.NAME_MAX_LENGTH);

            a.HasOne(x => x.Unit)
                .WithMany(x => x.Auras)
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            a.HasOne(dd => dd.Target)
                .WithMany()
                .HasForeignKey(ddg => ddg.TargetId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<DamageDone>(dd =>
        {
            dd.Property(p => p.Spell)
                .HasMaxLength(DamageDone.SPELL_MAX_LENGTH);

            dd.HasOne(dd => dd.Unit)
                .WithMany(cp => cp.DamageDones)
                .HasForeignKey(ddg => ddg.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            dd.HasOne(dd => dd.Target)
                .WithMany()
                .HasForeignKey(ddg => ddg.TargetId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<HealDone>(hd =>
        {
            hd.Property(p => p.Spell)
                .HasMaxLength(HealDone.SPELL_MAX_LENGTH);

            hd.HasOne(hd => hd.Unit)
                .WithMany(cp => cp.HealDones)
                .HasForeignKey(ddg => ddg.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            hd.HasOne(hd => hd.Target)
                .WithMany()
                .HasForeignKey(ddg => ddg.TargetId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ResourceRecovery>(rr =>
        {
            rr.Property(p => p.Spell)
                .HasMaxLength(ResourceRecovery.SPELL_MAX_LENGTH);

            rr.HasOne(rr => rr.Unit)
                .WithMany(cp => cp.ResourceRecoveries)
                .HasForeignKey(ddg => ddg.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            rr.HasOne(rr => rr.Target)
                .WithMany()
                .HasForeignKey(ddg => ddg.TargetId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
