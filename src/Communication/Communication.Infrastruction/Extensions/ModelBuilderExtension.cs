using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.Entities.Post;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Extensions;

internal static class ModelBuilderExtension
{
    public static void Creating(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>(c =>
        {
            c.Property(c => c.Name)
                .HasMaxLength(Community.NAME_MAX_LENGTH);

            c.Property(c => c.Description)
                .HasMaxLength(Community.DESCRIPTION_MAX_LENGTH);
        });

        modelBuilder.Entity<CommunityPost>(cp =>
        {
            cp.Property(cp => cp.Content)
                .HasMaxLength(CommunityPost.CONTENT_MAX_LENGTH);

            cp.HasOne(cp => cp.Community)
                .WithMany(c => c.CommunityPosts)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostComment>(cpc =>
        {
            cpc.Property(cp => cp.Content)
                .HasMaxLength(CommunityPostComment.CONTENT_MAX_LENGTH);

            cpc.HasOne(cpc => cpc.CommunityPost)
                .WithMany(c => c.CommunityPostComments)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostDislike>(cpd =>
        {
            cpd.HasOne(cpd => cpd.CommunityPost)
                .WithMany(c => c.CommunityPostDislikes)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostLike>(cpl =>
        {
            cpl.HasOne(cpl => cpl.CommunityPost)
                .WithMany(c => c.CommunityPostLikes)
                .HasForeignKey(uh => uh.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityUser>(cu =>
        {
            cu.HasOne(uh => uh.Community)
                .WithMany(c => c.CommunityUsers)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InviteToCommunity>(itc =>
        {
            itc.HasOne(itc => itc.Community)
                .WithMany(c => c.InvitesToCommunity)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityDiscussion>(cd =>
        {
            cd.Property(cd => cd.Title)
                .HasMaxLength(CommunityDiscussion.TITLE_MAX_LENGTH);

            cd.Property(cd => cd.Content)
                .HasMaxLength(CommunityDiscussion.CONTENT_MAX_LENGTH);

            cd.HasOne(cd => cd.Community)
                .WithMany(c => c.CommunityDiscussions)
                .HasForeignKey(uh => uh.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityDiscussionComment>(cdm =>
        {
            cdm.Property(cdm => cdm.Content)
                .HasMaxLength(CommunityDiscussionComment.CONTENT_MAX_LENGTH);

            cdm.HasOne(cdm => cdm.CommunityDiscussion)
                .WithMany(c => c.CommunityDiscussionComments)
                .HasForeignKey(uh => uh.CommunityDiscussionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPost>(up =>
        {
            up.Property(up => up.Content)
                .HasMaxLength(UserPost.CONTENT_MAX_LENGTH);
        });

        modelBuilder.Entity<UserPostComment>(upc =>
        {
            upc.Property(upc => upc.Content)
                .HasMaxLength(UserPostComment.CONTENT_MAX_LENGTH);

            upc.HasOne(upc => upc.UserPost)
                .WithMany(c => c.UserPostComments)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPostDislike>(upd =>
        {
            upd.HasOne(upd => upd.UserPost)
                .WithMany(c => c.UserPostDislikes)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPostLike>(upl =>
        {
            upl.HasOne(upl => upl.UserPost)
                .WithMany(c => c.UserPostLikes)
                .HasForeignKey(uh => uh.UserPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
