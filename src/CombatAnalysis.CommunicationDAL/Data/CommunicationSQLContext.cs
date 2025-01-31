using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Data;

public class CommunicationSQLContext : DbContext
{
    public CommunicationSQLContext(DbContextOptions<CommunicationSQLContext> options) : base(options)
    {
    }

    #region Community

    public DbSet<Community>? Community { get; }

    public DbSet<CommunityDiscussion>? CommunityDiscussion { get; }

    public DbSet<CommunityDiscussionComment>? CommunityDiscussionComment { get; }

    public DbSet<CommunityUser>? CommunityUser { get; }

    public DbSet<InviteToCommunity>? InviteToCommunity { get; }

    #endregion

    #region Post
    public DbSet<CommunityPost>? CommunityPost { get; }

    public DbSet<CommunityPostComment>? CommunityPostComment { get; }

    public DbSet<CommunityPostLike>? CommunityPostLike { get; }

    public DbSet<CommunityPostDislike>? CommunityPostDislike { get; }

    public DbSet<UserPost>? UserPost { get; }

    public DbSet<UserPostComment>? UserPostComment { get; }

    public DbSet<UserPostLike>? UserPostLike { get; }

    public DbSet<UserPostDislike>? UserPostDislike { get; }

    #endregion
}
