using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountCommunityPostDislike;

internal class CountCommunityPostDislikeHandler(ICommunityPostRepository repository) : IRequestHandler<CountCommunityPostDislikeQuery, int>
{
    private readonly ICommunityPostRepository _repository = repository;

    public async Task<int> Handle(CountCommunityPostDislikeQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountDislikeAsync(request.CommunityPostId, cancellationToken);

        return count;
    }
}
