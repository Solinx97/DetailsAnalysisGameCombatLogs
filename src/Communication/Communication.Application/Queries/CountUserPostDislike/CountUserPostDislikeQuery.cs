using MediatR;

namespace Communication.Application.Queries.CountUserPostDislike;

public record CountUserPostDislikeQuery(
    int UserPostId
    ) : IRequest<int>;
