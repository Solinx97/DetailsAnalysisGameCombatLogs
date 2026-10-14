using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.Entities.Post;
using Communication.Infrastruction.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Persistent;

internal class CommunicationContext(DbContextOptions<CommunicationContext> options) : DbContext(options)
{
    #region Community

    public DbSet<Community> Community => Set<Community>();

    public DbSet<CommunityDiscussion> CommunityDiscussion => Set<CommunityDiscussion>();

    public DbSet<CommunityDiscussionComment> CommunityDiscussionComment => Set<CommunityDiscussionComment>();

    public DbSet<CommunityUser> CommunityUser => Set<CommunityUser>();

    public DbSet<InviteToCommunity> InviteToCommunity => Set<InviteToCommunity>();

    #endregion

    #region Post
    public DbSet<CommunityPost> CommunityPost => Set<CommunityPost>();

    public DbSet<CommunityPostComment> CommunityPostComment => Set<CommunityPostComment>();

    public DbSet<CommunityPostLike> CommunityPostLike => Set<CommunityPostLike>();

    public DbSet<CommunityPostDislike> CommunityPostDislike => Set<CommunityPostDislike>();

    public DbSet<UserPost> UserPost => Set<UserPost>();

    public DbSet<UserPostComment> UserPostComment => Set<UserPostComment>();

    public DbSet<UserPostLike> UserPostLike => Set<UserPostLike>();

    public DbSet<UserPostDislike> UserPostDislike => Set<UserPostDislike>();

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Creating();
    }
}
