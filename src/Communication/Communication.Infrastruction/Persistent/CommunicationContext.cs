using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.Entities.Post;
using Communication.Infrastruction.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Persistent;

internal class CommunicationContext(DbContextOptions<CommunicationContext> options) : DbContext(options)
{
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Creating();
    }
}
