using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
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

        modelBuilder.Entity<CombatUnit>(uh =>
        {
            uh.Property(uh => uh.GameId)
                .HasMaxLength(CombatUnit.GAMEID_MAX_LENGTH);

            uh.HasOne(uh => uh.Combat)
                .WithMany(c => c.Units)
                .HasForeignKey(uh => uh.CombatId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UnitHealth>(uh =>
        {
            uh.Property(uh => uh.GameId)
                .HasMaxLength(UnitHealth.GAMEID_MAX_LENGTH);

            uh.HasOne(uh => uh.Combat)
                .WithMany(c => c.UnitHeaths)
                .HasForeignKey(uh => uh.CombatId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UnitPosition>(uh =>
        {
            uh.Property(uh => uh.GameId)
                .HasMaxLength(UnitPosition.GAMEID_MAX_LENGTH);

            uh.HasOne(uh => uh.Combat)
                .WithMany(c => c.UnitPositions)
                .HasForeignKey(uh => uh.CombatId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        AddTableRefs(modelBuilder);
    }

    private static void AddTableRefs(ModelBuilder modelBuilder)
    {
        // CombatLog
        modelBuilder.Entity<CombatLog>(cl =>
        {
            cl.Property(p => p.Name)
                .HasMaxLength(CombatLog.NAME_MAX_LENGTH);
        });

        // Combat
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

        modelBuilder.Entity<CombatPlayerStats>(cps =>
        {
            cps.Property(p => p.Talents)
                .HasMaxLength(CombatPlayerStats.TALENTS_MAX_LENGTH);

            cps.HasOne(cps => cps.CombatPlayer)
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

        AddCombatPlayerDataTableRefs(modelBuilder);
    }

    private static void AddCombatPlayerDataTableRefs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CombatPlayerPreAura>(cpa =>
        {
            cpa.HasOne(a => a.CombatPlayer)
                .WithMany(cp => cp.PreAuras)
                .HasForeignKey(a => a.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CombatPlayerAura>(cpa =>
        {
            cpa.Property(p => p.Name)
                .HasMaxLength(CombatPlayerAura.NAME_MAX_LENGTH);

            cpa.Property(p => p.Creator)
                .HasMaxLength(CombatPlayerAura.CREATOR_MAX_LENGTH);

            cpa.Property(p => p.Target)
                .HasMaxLength(CombatPlayerAura.TARGET_MAX_LENGTH);

            cpa.HasOne(a => a.CombatPlayer)
                .WithMany(cp => cp.Auras)
                .HasForeignKey(a => a.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CombatPlayerCast>(dd =>
        {
            dd.Property(p => p.Spell)
                .HasMaxLength(CombatPlayerCast.SPELL_MAX_LENGTH);

            dd.Property(p => p.Creator)
                .HasMaxLength(CombatPlayerCast.CREATOR_MAX_LENGTH);

            dd.Property(p => p.Target)
                .HasMaxLength(CombatPlayerCast.TARGET_MAX_LENGTH);

            dd.HasOne(cpc => cpc.CombatPlayer)
                .WithMany(cp => cp.Casts)
                .HasForeignKey(cpc => cpc.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DamageDone>(dd =>
        {
            dd.Property(p => p.Spell)
                .HasMaxLength(DamageDone.SPELL_MAX_LENGTH);

            dd.Property(p => p.Creator)
                .HasMaxLength(DamageDone.CREATOR_MAX_LENGTH);

            dd.Property(p => p.Target)
                .HasMaxLength(DamageDone.TARGET_MAX_LENGTH);

            dd.HasOne(dd => dd.CombatPlayer)
                .WithMany(cp => cp.DamageDones)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DamageDoneGeneral>(ddg =>
        {
            ddg.Property(p => p.Spell)
                .HasMaxLength(DamageDoneGeneral.SPELL_MAX_LENGTH);

            ddg.HasOne(ddg => ddg.CombatPlayer)
                .WithMany(cp => cp.DamageDoneGenerals)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HealDone>(hd =>
        {
            hd.Property(p => p.Spell)
                .HasMaxLength(HealDone.SPELL_MAX_LENGTH);

            hd.Property(p => p.Creator)
                .HasMaxLength(HealDone.CREATOR_MAX_LENGTH);

            hd.Property(p => p.Target)
                .HasMaxLength(HealDone.TARGET_MAX_LENGTH);

            hd.HasOne(hd => hd.CombatPlayer)
                .WithMany(cp => cp.HealDones)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HealDoneGeneral>(hdg =>
        {
            hdg.Property(p => p.Spell)
                .HasMaxLength(HealDoneGeneral.SPELL_MAX_LENGTH);

            hdg.HasOne(hdg => hdg.CombatPlayer)
                .WithMany(cp => cp.HealDoneGenerals)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DamageTaken>(dt =>
        {
            dt.Property(p => p.Spell)
                .HasMaxLength(DamageTaken.SPELL_MAX_LENGTH);

            dt.Property(p => p.Creator)
                .HasMaxLength(DamageTaken.CREATOR_MAX_LENGTH);

            dt.Property(p => p.Target)
                .HasMaxLength(DamageTaken.TARGET_MAX_LENGTH);

            dt.HasOne(dt => dt.CombatPlayer)
                .WithMany(cp => cp.DamageTakens)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DamageTakenGeneral>(dtg =>
        {
            dtg.Property(p => p.Spell)
                .HasMaxLength(DamageTakenGeneral.SPELL_MAX_LENGTH);

            dtg.HasOne(dtg => dtg.CombatPlayer)
                .WithMany(cp => cp.DamageTakenGenerals)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ResourceRecovery>(rr =>
        {
            rr.Property(p => p.Spell)
                .HasMaxLength(ResourceRecovery.SPELL_MAX_LENGTH);

            rr.Property(p => p.Creator)
                .HasMaxLength(ResourceRecovery.CREATOR_MAX_LENGTH);

            rr.Property(p => p.Target)
                .HasMaxLength(ResourceRecovery.TARGET_MAX_LENGTH);

            rr.HasOne(rr => rr.CombatPlayer)
                .WithMany(cp => cp.ResourceRecoveries)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ResourceRecoveryGeneral>(rrg =>
        {
            rrg.Property(p => p.Spell)
                .HasMaxLength(ResourceRecoveryGeneral.SPELL_MAX_LENGTH);

            rrg.HasOne(rrg => rrg.CombatPlayer)
                .WithMany(cp => cp.ResourceRecoveryGenerals)
                .HasForeignKey(ddg => ddg.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CombatPlayerDeath>(cpd =>
        {
            cpd.Property(p => p.Username)
                .HasMaxLength(CombatPlayerDeath.USERNAME_MAX_LENGTH);

            cpd.Property(p => p.LastHitSpell)
                .HasMaxLength(CombatPlayerDeath.SPELL_MAX_LENGTH);

            cpd.HasOne(cpd => cpd.CombatPlayer)
                .WithMany(cp => cp.CombatPlayerDeathes)
                .HasForeignKey(cpd => cpd.CombatPlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
