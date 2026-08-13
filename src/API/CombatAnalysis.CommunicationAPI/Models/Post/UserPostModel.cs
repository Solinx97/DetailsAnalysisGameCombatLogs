using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Post;

public record UserPostModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Owner,
    [Required] string Content,
    int PublicType,
    string Tags,
    [Required] DateTimeOffset CreatedAt,
    [Required] string AppUserId,
    [Range(0, int.MaxValue)] int LikeCount,
    [Range(0, int.MaxValue)] int DislikeCount,
    [Range(0, int.MaxValue)] int CommentCount
    );
