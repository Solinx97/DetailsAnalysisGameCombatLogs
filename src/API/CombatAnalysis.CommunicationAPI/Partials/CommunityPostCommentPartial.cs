using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Partials;

public record CommunityPostCommentPartial(
    [Range(0, int.MaxValue)] int Id,
    [Range(0, int.MaxValue)] int CommunityPostId,
    [Required] string Content
    );
