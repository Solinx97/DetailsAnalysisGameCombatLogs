using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.Entities.Post;
using Communication.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Extensions;

internal static class ModelBuilderExtension
{
    public static void Creating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new CommunityId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(c => c.Name)
                .HasMaxLength(Community.NAME_MAX_LENGTH);

            builder.Property(c => c.Description)
                .HasMaxLength(Community.DESCRIPTION_MAX_LENGTH);
        });

        modelBuilder.Entity<CommunityPost>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new CommunityPostId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.CommunityId).HasConversion(
                           id => id.Value,
                           value => new CommunityId(value)
                       );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(cp => cp.Content)
                .HasMaxLength(CommunityPost.CONTENT_MAX_LENGTH);

            builder.HasOne(cp => cp.Community)
                .WithMany(c => c.CommunityPosts)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostComment>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new CommunityPostCommentId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.CommunityId).HasConversion(
                           id => id.Value,
                           value => new CommunityId(value)
                       );

            builder.Property(c => c.CommunityPostId).HasConversion(
                            id => id.Value,
                            value => new CommunityPostId(value)
                        );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(cp => cp.Content)
                .HasMaxLength(CommunityPostComment.CONTENT_MAX_LENGTH);

            builder.HasOne(cpc => cpc.CommunityPost)
                .WithMany(c => c.CommunityPostComments)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostDislike>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new CommunityPostDislikeId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.CommunityId).HasConversion(
                           id => id.Value,
                           value => new CommunityId(value)
                       );

            builder.Property(c => c.CommunityPostId).HasConversion(
                            id => id.Value,
                            value => new CommunityPostId(value)
                        );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.HasOne(cpd => cpd.CommunityPost)
                .WithMany(c => c.CommunityPostDislikes)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostLike>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new CommunityPostLikeId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.CommunityId).HasConversion(
                           id => id.Value,
                           value => new CommunityId(value)
                       );

            builder.Property(c => c.CommunityPostId).HasConversion(
                            id => id.Value,
                            value => new CommunityPostId(value)
                        );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.HasOne(cpl => cpl.CommunityPost)
                .WithMany(c => c.CommunityPostLikes)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityUser>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new CommunityUserId(value)
                       );

            builder.Property(c => c.CommunityId).HasConversion(
                           id => id.Value,
                           value => new CommunityId(value)
                       );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.HasOne(uh => uh.Community)
                .WithMany(c => c.CommunityUsers)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InviteToCommunity>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new InviteToCommunityId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.CommunityId).HasConversion(
                           id => id.Value,
                           value => new CommunityId(value)
                       );

            builder.Property(c => c.ToAppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.HasOne(itc => itc.Community)
                .WithMany(c => c.InvitesToCommunity)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityDiscussion>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new CommunityDiscussionId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.CommunityId).HasConversion(
                           id => id.Value,
                           value => new CommunityId(value)
                       );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(cd => cd.Title)
                .HasMaxLength(CommunityDiscussion.TITLE_MAX_LENGTH);

            builder.Property(cd => cd.Content)
                .HasMaxLength(CommunityDiscussion.CONTENT_MAX_LENGTH);

            builder.HasOne(cd => cd.Community)
                .WithMany(c => c.CommunityDiscussions)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityDiscussionComment>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new CommunityDiscussionCommentId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.CommunityDiscussionId).HasConversion(
                           id => id.Value,
                           value => new CommunityDiscussionId(value)
                       );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(cdm => cdm.Content)
                .HasMaxLength(CommunityDiscussionComment.CONTENT_MAX_LENGTH);

            builder.HasOne(cdm => cdm.CommunityDiscussion)
                .WithMany(c => c.CommunityDiscussionComments)
                .HasForeignKey(uh => uh.CommunityDiscussionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPost>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new UserPostId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(up => up.Content)
                .HasMaxLength(UserPost.CONTENT_MAX_LENGTH);
        });

        modelBuilder.Entity<UserPostComment>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new UserPostCommentId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.UserPostId).HasConversion(
                           id => id.Value,
                           value => new UserPostId(value)
                       );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.Property(upc => upc.Content)
                .HasMaxLength(UserPostComment.CONTENT_MAX_LENGTH);

            builder.HasOne(upc => upc.UserPost)
                .WithMany(c => c.UserPostComments)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPostDislike>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new UserPostDislikeId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.UserPostId).HasConversion(
                           id => id.Value,
                           value => new UserPostId(value)
                       );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.HasOne(upd => upd.UserPost)
                .WithMany(c => c.UserPostDislikes)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPostLike>(builder =>
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                           id => id.Value,
                           value => new UserPostLikeId(value)
                       ).ValueGeneratedOnAdd();

            builder.Property(c => c.UserPostId).HasConversion(
                           id => id.Value,
                           value => new UserPostId(value)
                       );

            builder.Property(c => c.AppUserId).HasConversion(
                           id => id.Value,
                           value => new UserId(value)
                       );

            builder.HasOne(upl => upl.UserPost)
                .WithMany(c => c.UserPostLikes)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
