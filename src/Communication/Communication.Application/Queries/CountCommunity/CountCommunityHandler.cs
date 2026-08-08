using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.CountCommunity;

internal class CountCommunityHandler(ICommunityRepository repository) : IRequestHandler<CountCommunityQuery, int>
{
    private readonly ICommunityRepository _repository = repository;

    public async Task<int> Handle(CountCommunityQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(cancellationToken);

        return count;
    }
}
