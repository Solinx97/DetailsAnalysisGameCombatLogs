using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountCommunityPostComment;

internal class CountCommunityPostCommentHandler(ICommunityPostRepository repository) : IRequestHandler<CountCommunityPostCommentQuery, int>
{
    private readonly ICommunityPostRepository _repository = repository;

    public async Task<int> Handle(CountCommunityPostCommentQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountCommentAsync(request.CommunityPostId, cancellationToken);

        return count;
    }
}
