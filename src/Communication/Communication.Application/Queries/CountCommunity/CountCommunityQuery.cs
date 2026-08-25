using MediatR;

namespace Communication.Application.Queries.CountCommunity;

public record CountCommunityQuery(
    ) : IRequest<int>;
