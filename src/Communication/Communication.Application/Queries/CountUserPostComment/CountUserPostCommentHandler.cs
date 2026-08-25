using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountUserPostComment;

internal class CountUserPostCommentHandler(IUserPostRepository repository) : IRequestHandler<CountUserPostCommentQuery, int>
{
    private readonly IUserPostRepository _repository = repository;

    public async Task<int> Handle(CountUserPostCommentQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountCommentAsync(request.UserPostId, cancellationToken);

        return count;
    }
}
