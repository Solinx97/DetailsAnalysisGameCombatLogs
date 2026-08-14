namespace Communication.Domain.ReadModel;

public sealed record UserPostReadModel(
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
    int Reaction
    );