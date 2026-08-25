using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CanJoinToCommunity;

internal class CanJoinToCommunityHandler(ICommunityRepository repository) : IRequestHandler<CanJoinToCommunityQuery, bool>
{
    private readonly ICommunityRepository _repository = repository;

    public async Task<bool> Handle(CanJoinToCommunityQuery request, CancellationToken cancellationToken)
    {
        var canJoin = await _repository.CanJoinAsync(request.AppUserId, request.CommunityId, cancellationToken);

        return canJoin;
    }
}
