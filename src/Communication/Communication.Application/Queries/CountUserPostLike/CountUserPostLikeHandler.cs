using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountUserPostLike;

internal class CountUserPostLikeHandler(IUserPostRepository repository) : IRequestHandler<CountUserPostLikeQuery, int>
{
    private readonly IUserPostRepository _repository = repository;

    public async Task<int> Handle(CountUserPostLikeQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountLikeAsync(request.UserPostId, cancellationToken);

        return count;
    }
}

