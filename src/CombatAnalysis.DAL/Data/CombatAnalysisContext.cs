using CombatAnalysis.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data
{
    public class CombatAnalysisContext : DbContext
    {
        public CombatAnalysisContext(
            DbContextOptions<CombatAnalysisContext> options) : base(options)
        {
        }

        public DbSet<Combat> Combat { get; set; }
    }
}
