using MediatR;

namespace Communication.Application.Queries.CountUserPostComment;

public record CountUserPostCommentQuery(
    int UserPostId
    ) : IRequest<int>;
