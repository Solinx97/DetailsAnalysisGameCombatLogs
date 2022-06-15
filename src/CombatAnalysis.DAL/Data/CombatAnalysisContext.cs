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

        public DbSet<Combat> Combat { get; set; }

        public DbSet<CombatPlayerData> CombatPlayerData { get; set; }
    }
}
