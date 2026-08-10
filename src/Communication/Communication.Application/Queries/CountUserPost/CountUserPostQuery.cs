using MediatR;

namespace Communication.Application.Queries.CountUserPost;

public record CountUserPostQuery(
    string AppUserId
    ) : IRequest<int>;
