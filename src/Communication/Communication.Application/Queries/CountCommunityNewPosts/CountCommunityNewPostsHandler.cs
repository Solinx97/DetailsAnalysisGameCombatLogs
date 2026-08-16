using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountCommunityNewPosts;

internal class CountCommunityNewPostsHandler(ICommunityPostRepository repository) : IRequestHandler<CountCommunityNewPostsQuery, int>
{
    private readonly ICommunityPostRepository _repository = repository;

    public async Task<int> Handle(CountCommunityNewPostsQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountNewPostsAsync(request.CommunityId, request.LastCheck, cancellationToken);

        return count;
    }
}
