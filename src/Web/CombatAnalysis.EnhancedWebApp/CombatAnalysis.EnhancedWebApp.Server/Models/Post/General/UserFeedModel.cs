using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post.General;

public record UserFeedModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Owner,
    [Required] string Content,
    int PublicType,
    string Tags,
    [Required] DateTimeOffset CreatedAt,
    [Required] string AppUserId,
    string? CommunityName,
    int? PostType,
    int? Restrictions,
    int? CommunityId
    );
