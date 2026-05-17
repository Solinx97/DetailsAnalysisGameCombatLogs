using CombatAnalysis.UserDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserDAL.Data;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public DbSet<AppUser>? AppUser { get; }

    public DbSet<Customer>? Customer { get; }

    public DbSet<BannedUser>? BannedUser { get; }

    public DbSet<Friend>? Friend { get; }

    public DbSet<RequestToConnect>? RequestToConnet { get; }
}
