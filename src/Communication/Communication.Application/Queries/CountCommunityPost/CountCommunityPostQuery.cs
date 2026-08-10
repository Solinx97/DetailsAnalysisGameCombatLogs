using MediatR;

namespace Communication.Application.Queries.CountCommunityPost;

public record CountCommunityPostQuery(
    int CommunityId
    ) : IRequest<int>;
