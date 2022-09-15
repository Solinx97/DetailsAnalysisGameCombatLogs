using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Data
{
    public class SQLContext : DbContext
    {
        public SQLContext(DbContextOptions<SQLContext> options) : base(options)
        {
            var isExists = Database.EnsureCreated();

            if (isExists)
            {
                Task.Run(() => DbProcedureHelper.CreateProceduresAsync(this));
            }
        }

        public DbSet<AppUser> AppUser { get; set; }

        public DbSet<PersonalChat> PersonalChat { get; set; }

        public DbSet<PersonalChatMessage> PersonalChatMessage { get; set; }

        public DbSet<InviteToGroupChat> InviteToGroupChat { get; set; }

        public DbSet<GroupChat> GroupChat { get; set; }

        public DbSet<GroupChatMessage> GroupChatMessage { get; set; }

        public DbSet<GroupChatUser> GroupChatUser { get; set; }

        public DbSet<BannedUser> BannedUser { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

        public DbSet<CombatLog> CombatLog { get; set; }

        public DbSet<CombatLogByUser> CombatLogByUser { get; set; }

        public DbSet<Combat> Combat { get; set; }

        public DbSet<CombatPlayer> CombatPlayer { get; set; }

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
