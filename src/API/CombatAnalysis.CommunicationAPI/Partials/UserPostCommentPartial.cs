using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Partials;

public record UserPostCommentPartial(
    [Range(0, int.MaxValue)] int Id,
    [Range(0, int.MaxValue)] int UserPostId,
    [Required] string Content
    );
