namespace Communication.Domain.ReadModel;

public sealed record UserPostReadModel(
    int Id,
    string Content,
    int PublicType,
    string Tags,
    DateTimeOffset CreatedAt,
    Guid AppUserId,
    int LikeCount,
    int DislikeCount,
    int CommentCount,
    int Reaction
    );