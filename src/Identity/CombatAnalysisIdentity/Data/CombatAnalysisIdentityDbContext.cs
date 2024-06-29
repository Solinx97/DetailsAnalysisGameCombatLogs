using Microsoft.EntityFrameworkCore;

namespace CombatAnalysisIdentity.Data;

public class CombatAnalysisIdentityDbContext : DbContext
{
    public CombatAnalysisIdentityDbContext(DbContextOptions<CombatAnalysisIdentityDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
