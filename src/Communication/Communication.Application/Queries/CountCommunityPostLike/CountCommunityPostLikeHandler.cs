using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountCommunityPostLike;

internal class CountCommunityPostLikeHandler(ICommunityPostRepository repository) : IRequestHandler<CountCommunityPostLikeQuery, int>
{
    private readonly ICommunityPostRepository _repository = repository;

    public async Task<int> Handle(CountCommunityPostLikeQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountLikeAsync(request.CommunityPostId, cancellationToken);

        return count;
    }
}
