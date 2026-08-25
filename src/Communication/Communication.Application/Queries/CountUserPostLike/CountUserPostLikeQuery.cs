using MediatR;

namespace Communication.Application.Queries.CountUserPostLike;

public record CountUserPostLikeQuery(
    int UserPostId
    ) : IRequest<int>;
