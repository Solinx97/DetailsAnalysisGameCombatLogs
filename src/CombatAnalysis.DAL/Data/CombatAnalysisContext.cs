using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
                Task.Run(async () => await DbProcedureHelper.CreateProceduresAsync(this));
            }
        }

        public DbSet<User> User { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

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
