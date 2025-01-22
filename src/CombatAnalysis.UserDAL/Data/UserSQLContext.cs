using CombatAnalysis.UserDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserDAL.Data;

public class UserSQLContext : DbContext
{
    public UserSQLContext(DbContextOptions<UserSQLContext> options) : base(options)
    {
    }

    #region Customer

    public DbSet<AppUser>? AppUser { get; }

    public DbSet<Customer>? Customer { get; }

    public DbSet<BannedUser>? BannedUser { get; }

    public DbSet<Friend>? Friend { get; }

    public DbSet<RequestToConnect>? RequestToConnet { get; }

    #endregion
}
