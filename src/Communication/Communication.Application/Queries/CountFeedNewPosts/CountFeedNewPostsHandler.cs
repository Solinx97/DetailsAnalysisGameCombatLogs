using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountFeedNewPosts;

internal class CountFeedNewPostsHandler(IUserFeedRepository repository) : IRequestHandler<CountFeedNewPostsQuery, int>
{
    private readonly IUserFeedRepository _repository = repository;

    public async Task<int> Handle(CountFeedNewPostsQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountNewPostsAsync(request.AppUserId, request.FriendsId, request.LastCheck, cancellationToken);

        return count;
    }
}
