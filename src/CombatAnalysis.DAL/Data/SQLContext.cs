﻿using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Entities.Community;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data;

public class SQLContext : DbContext
{
    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
        var isExists = Database.EnsureCreated();
        if (isExists)
        {
            DbProcedureHelper.CreateProcedures(this);
        }
    }

    public DbSet<AppUser>? AppUser { get; }

    public DbSet<RefreshToken>? RefreshToken { get; }

    public DbSet<PersonalChat>? PersonalChat { get; }

    #region Communication

    public DbSet<Community>? Community { get; }

    public DbSet<CommunityPost>? CommunityPost { get; }

    public DbSet<CommunityPostComment>? CommunityPostComment { get; }

    public DbSet<CommunityPostDislike>? CommunityPostDislike { get; }

    public DbSet<CommunityPostLike>? CommunityPostLike { get; }

    public DbSet<CommunityUser>? CommunityUser { get; }

    public DbSet<InviteToCommunity>? InviteToCommunity { get; }

    #endregion

    #region Chat

    public DbSet<PersonalChatMessage>? PersonalChatMessage { get; }

    public DbSet<InviteToGroupChat>? InviteToGroupChat { get; }

    public DbSet<GroupChat>? GroupChat { get; }

    public DbSet<GroupChatMessage>? GroupChatMessage { get; }

    public DbSet<GroupChatUser>? GroupChatUser { get; }

    #endregion

    #region User

    public DbSet<Customer>? Customer { get; }

    public DbSet<BannedUser>? BannedUser { get; }

    public DbSet<Friend>? Friend { get; }

    public DbSet<Post>? Post { get; }

    public DbSet<PostLike>? PostLike { get; }

    public DbSet<PostDislike>? PostDislike { get; }

    public DbSet<RequestToConnect>? RequestToConnet { get; }

    #endregion

    #region Combat details

    public DbSet<CombatLog>? CombatLog { get; }

    public DbSet<CombatLogByUser>? CombatLogByUser { get; }

    public DbSet<Combat>? Combat { get; }

    public DbSet<CombatPlayer>? CombatPlayer { get; }

    public DbSet<DamageDone>? DamageDone { get; }

    public DbSet<DamageDoneGeneral>? DamageDoneGeneral { get; }

    public DbSet<HealDone>? HealDone { get; }

    public DbSet<HealDoneGeneral>? HealDoneGeneral { get; }

    public DbSet<DamageTaken>? DamageTaken { get; }

    public DbSet<DamageTakenGeneral>? DamageTakenGeneral { get; }

    public DbSet<ResourceRecovery>? ResourceRecovery { get; }

    public DbSet<ResourceRecoveryGeneral>? ResourceRecoveryGeneral { get; }

    #endregion
}