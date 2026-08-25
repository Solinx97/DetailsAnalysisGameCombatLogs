using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountUserPostDislike;

internal class CountUserPostDislikeHandler(IUserPostRepository repository) : IRequestHandler<CountUserPostDislikeQuery, int>
{
    private readonly IUserPostRepository _repository = repository;

    public async Task<int> Handle(CountUserPostDislikeQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountDislikeAsync(request.UserPostId, cancellationToken);

        return count;
    }
}

