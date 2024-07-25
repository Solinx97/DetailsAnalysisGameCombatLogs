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

    public DbSet<RefreshToken> RefreshToken { get; set; }

    public DbSet<IdentityUser> IdentityUser { get; set; }

    public DbSet<Client> Client { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                Id = "client1",
                RedirectUrl = "encounters.analysis.com/callback",
                Scope = "client1scope",
                ClientName = "web",
                ClientType = "public",
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            },
            new Client
            {
                Id = "client2",
                RedirectUrl = "localhost:45571/callback",
                Scope = "client2scope",
                ClientName = "desktop",
                ClientType = "public",
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            },
            new Client
            {
                Id = "client3",
                RedirectUrl = "localhost:44479/callback",
                Scope = "client3scope",
                ClientName = "devWeb",
                ClientType = "public",
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            }
        );
    }
}
