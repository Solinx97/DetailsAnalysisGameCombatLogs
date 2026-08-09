using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Community;

public record CommunityDiscussionCommentModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Content,
    [Required] DateTimeOffset CreatedAt,
    [Required] string AppUserId,
    [Range(0, int.MaxValue)] int CommunityDiscussionId
    );
