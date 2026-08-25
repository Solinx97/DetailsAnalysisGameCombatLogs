using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountUserPost;

internal class CountUserPostHandler(IUserPostRepository repository) : IRequestHandler<CountUserPostQuery, int>
{
    private readonly IUserPostRepository _repository = repository;

    public async Task<int> Handle(CountUserPostQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.AppUserId, cancellationToken);

        return count;
    }
}
