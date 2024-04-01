using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Entities.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CustomerDAL.Data;

public class SQLContext : DbContext
{
    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Secret>? Secrets { get; }

    public DbSet<AppUser>? AppUser { get; }

    public DbSet<Customer>? Customer { get; }

    public DbSet<BannedUser>? BannedUser { get; }

    public DbSet<Friend>? Friend { get; }

    public DbSet<RequestToConnect>? RequestToConnet { get; }
}
