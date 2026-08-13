namespace Communication.Domain.ReadModel;

public sealed record CommunityPostReadModel(
    int Id,
    string CommunityName,
    string Owner,
    string Content,
    int PostType,
    int PublicType,
    int Restrictions,
    string Tags,
    DateTimeOffset CreatedAt,
    int CommunityId,
    string AppUserId,
    int LikeCount,
    int DislikeCount,
    int CommentCount
    );
