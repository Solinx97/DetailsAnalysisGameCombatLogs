using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountCommunityPost;

internal class CountCommunityPostHandler(ICommunityPostRepository repository) : IRequestHandler<CountCommunityPostQuery, int>
{
    private readonly ICommunityPostRepository _repository = repository;

    public async Task<int> Handle(CountCommunityPostQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CommunityId, cancellationToken);

        return count;
    }
}