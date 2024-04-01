using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Data;

public class SQLContext : DbContext
{
    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    #region Community

    public DbSet<Community>? Community { get; }

    public DbSet<CommunityDiscussion>? CommunityDiscussion { get; }

    public DbSet<CommunityDiscussionComment>? CommunityDiscussionComment { get; }

    public DbSet<CommunityUser>? CommunityUser { get; }

    public DbSet<InviteToCommunity>? InviteToCommunity { get; }

    #endregion

    #region Post

    public DbSet<Post>? Post { get; }

    public DbSet<PostComment>? PostComment { get; }

    public DbSet<PostLike>? PostLike { get; }

    public DbSet<PostDislike>? PostDislike { get; }

    public DbSet<CommunityPost>? CommunityPost { get; }

    public DbSet<UserPost>? UserPost { get; }

    #endregion
}
