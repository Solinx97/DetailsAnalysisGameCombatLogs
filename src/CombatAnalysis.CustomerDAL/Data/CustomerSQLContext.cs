﻿using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Entities.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CustomerDAL.Data;

public class CustomerSQLContext : DbContext
{
    public CustomerSQLContext(DbContextOptions<CustomerSQLContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    #region Customer

    public DbSet<Secret>? Secrets { get; }

    public DbSet<AppUser>? AppUser { get; }

    public DbSet<Customer>? Customer { get; }

    public DbSet<BannedUser>? BannedUser { get; }

    public DbSet<Friend>? Friend { get; }

    public DbSet<RequestToConnect>? RequestToConnet { get; }

    #endregion
}