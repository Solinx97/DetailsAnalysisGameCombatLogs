using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.Entities.Post;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Extensions;

internal static class ModelBuilderExtension
{
    public static void Creating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>(cl =>
        {
            cl.Property(p => p.Name)
                .HasMaxLength(Community.NAME_MAX_LENGTH);
        });

        modelBuilder.Entity<CommunityPost>(uh =>
        {
            uh.Property(uh => uh.CommunityName)
                .HasMaxLength(CommunityPost.COMMUNITY_NAME_MAX_LENGTH);

            uh.HasOne(uh => uh.Community)
                .WithMany(c => c.CommunityPosts)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostComment>(uh =>
        {
            uh.HasOne(uh => uh.CommunityPost)
                .WithMany(c => c.CommunityPostComments)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostDislike>(uh =>
        {
            uh.HasOne(uh => uh.CommunityPost)
                .WithMany(c => c.CommunityPostDislikes)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostLike>(uh =>
        {
            uh.HasOne(uh => uh.CommunityPost)
                .WithMany(c => c.CommunityPostLikes)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityUser>(uh =>
        {
            uh.HasOne(uh => uh.Community)
                .WithMany(c => c.CommunityUsers)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InviteToCommunity>(uh =>
        {
            uh.HasOne(uh => uh.Community)
                .WithMany(c => c.InvitesToCommunity)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityDiscussion>(uc =>
        {
            uc.Property(p => p.Title)
                .HasMaxLength(CommunityDiscussion.TITLE_MAX_LENGTH);

            uc.HasOne(uh => uh.Community)
                .WithMany(c => c.CommunityDiscussions)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityDiscussionComment>(uh =>
        {
            uh.HasOne(uh => uh.CommunityDiscussion)
                .WithMany(c => c.CommunityDiscussionComments)
                .HasForeignKey(uh => uh.CommunityDiscussionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPostComment>(uh =>
        {
            uh.HasOne(uh => uh.UserPost)
                .WithMany(c => c.UserPostComments)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPostDislike>(uh =>
        {
            uh.HasOne(uh => uh.UserPost)
                .WithMany(c => c.UserPostDislikes)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPostLike>(uh =>
        {
            uh.HasOne(uh => uh.UserPost)
                .WithMany(c => c.UserPostLikes)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
