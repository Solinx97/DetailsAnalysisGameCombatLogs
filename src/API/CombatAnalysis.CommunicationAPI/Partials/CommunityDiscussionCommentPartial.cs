using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Partials;

public record CommunityDiscussionCommentPartial(
    [Range(0, int.MaxValue)] int Id,
    [Range(0, int.MaxValue)] int CommunityDiscussionId,
    [Required] string Content
    );