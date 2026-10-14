using MediatR;

namespace Communication.Application.Queries.CountUserPost;

public record CountUserPostQuery(
    Guid AppUserId
    ) : IRequest<int>;
