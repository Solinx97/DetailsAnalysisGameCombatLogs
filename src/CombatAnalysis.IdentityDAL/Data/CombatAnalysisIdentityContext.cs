using CombatAnalysis.IdentityDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Data;

public class CombatAnalysisIdentityContext : DbContext
{
    public CombatAnalysisIdentityContext(DbContextOptions<CombatAnalysisIdentityContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<AuthorizationCodeChallenge> AuthorizationCodeChallenge { get; set; }

    public DbSet<IdentityUser> IdentityUser { get; set; }
}
