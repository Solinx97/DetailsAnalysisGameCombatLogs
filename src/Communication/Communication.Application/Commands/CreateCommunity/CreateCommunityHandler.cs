using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunity;

internal class CreateCommunityHandler(IGenericRepository<Community, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunityCommand, Community>
{
    private readonly IGenericRepository<Community, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Community> Handle(CreateCommunityCommand request, CancellationToken cancelationToken)
    {
        var community = Community.Create(request.Name, request.Description, request.PolicyType, request.AppUserId);
        await _repository.AddAsync(community, cancelationToken);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        return community;
    }
}
