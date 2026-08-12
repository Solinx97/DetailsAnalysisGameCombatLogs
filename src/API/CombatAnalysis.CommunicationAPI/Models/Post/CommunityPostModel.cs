using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Post;

public record CommunityPostModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] string CommunityName,
    [Required] string Owner,
    [Required] string Content,
    int PostType,
    int PublicType,
    int Restrictions,
    string Tags,
    [Required] DateTimeOffset CreatedAt,
    [Range(0, int.MaxValue)] int CommunityId,
    [Required] string AppUserId
    );
