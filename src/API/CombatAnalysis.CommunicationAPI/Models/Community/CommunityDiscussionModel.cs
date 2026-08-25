using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Community;

public record CommunityDiscussionModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Title,
    [Required] string Content,
    [Required] DateTimeOffset CreatedAt,
    [Required] string AppUserId,
    [Range(0, int.MaxValue)] int CommunityId
    );
