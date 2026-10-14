using AutoMapper;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using MediatR;

namespace Communication.Application.Queries.CountFeedNewPosts;

internal class CountFeedNewPostsHandler(IUserFeedRepository repository, IMapper mapper) : IRequestHandler<CountFeedNewPostsQuery, int>
{
    private readonly IUserFeedRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<int> Handle(CountFeedNewPostsQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountNewPostsAsync(request.AppUserId, [.. request.FriendsId.Select(x => new UserId(x))], request.LastCheck, cancellationToken);

        return count;
    }
}
