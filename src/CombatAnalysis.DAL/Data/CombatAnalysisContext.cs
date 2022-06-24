using CombatAnalysis.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data
{
    public class CombatAnalysisContext : DbContext
    {
        public CombatAnalysisContext(
            DbContextOptions<CombatAnalysisContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CombatLog> CombatLog { get; set; }

        public DbSet<Combat> Combat { get; set; }

        public DbSet<CombatPlayerData> CombatPlayerData { get; set; }

        public DbSet<DamageDone> DamageDone { get; set; }

        public DbSet<DamageDoneGeneral> DamageDoneGeneral { get; set; }

        public DbSet<HealDone> HealDone { get; set; }

        public DbSet<HealDoneGeneral> HealDoneGeneral { get; set; }

        public DbSet<DamageTaken> DamageTaken { get; set; }

        public DbSet<ResourceRecovery> ResourceRecovery { get; set; }
    }
}
