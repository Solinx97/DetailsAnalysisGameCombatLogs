using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Post;

public record UserPostModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Owner,
    [Required] string Content,
    int PublicType,
    string Tags,
    [Required] DateTimeOffset CreatedAt,
    int LikeCount,
    int DislikeCount,
    int CommentCount,
    [Required] string AppUserId
    );
