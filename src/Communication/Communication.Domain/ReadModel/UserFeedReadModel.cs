namespace Communication.Domain.ReadModel;

public sealed record UserFeedReadModel(
    int Id,
    string Owner,
    string Content,
    int PublicType,
    string Tags,
    DateTimeOffset CreatedAt,
    string AppUserId,
    int LikeCount,
    int DislikeCount,
    int CommentCount,
    string? CommunityName,
    int? PostType,
    int? Restrictions,
    int? CommunityId
    );
