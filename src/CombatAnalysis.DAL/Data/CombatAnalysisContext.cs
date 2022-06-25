using CombatAnalysis.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data
{
    public class CombatAnalysisContext : DbContext
    {
        public CombatAnalysisContext(
            DbContextOptions<CombatAnalysisContext> options) : base(options)
        {
            var isExists = Database.EnsureCreated();

            if (isExists)
            {
                var query = @"CREATE PROCEDURE GetCombatByCombatLogId (@combatLogId INT)
                                    AS SELECT Id, DungeonName, Name, EnergyRecovery, DamageDone, HealDone, DamageTaken, DeathNumber, UsedBuffs, IsWin, StartDate, FinishDate, CombatLogId
                                    FROM Combat
                                    WHERE CombatLogId = @combatLogId";

                Database.ExecuteSqlRaw(query);
            }
        }

        public DbSet<CombatLog> CombatLog { get; set; }

        public DbSet<Combat> Combat { get; set; }

        public DbSet<CombatPlayerData> CombatPlayerData { get; set; }

        public DbSet<DamageDone> DamageDone { get; set; }

        public DbSet<DamageDoneGeneral> DamageDoneGeneral { get; set; }

        public DbSet<HealDone> HealDone { get; set; }

        public DbSet<HealDoneGeneral> HealDoneGeneral { get; set; }

        public DbSet<DamageTaken> DamageTaken { get; set; }

        public DbSet<DamageTakenGeneral> DamageTakenGeneral { get; set; }

        public DbSet<ResourceRecovery> ResourceRecovery { get; set; }

        public DbSet<ResourceRecoveryGeneral> ResourceRecoveryGeneral { get; set; }
    }
}
