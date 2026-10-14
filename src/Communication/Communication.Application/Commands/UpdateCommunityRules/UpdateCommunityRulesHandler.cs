using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using MediatR;

namespace Communication.Application.Commands.UpdateCommunityRules;

internal class UpdateCommunityRulesHandler(IGenericRepository<Community, CommunityId> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommunityRulesCommand>
{
    private readonly IGenericRepository<Community, CommunityId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommunityRulesCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetByIdAsync(request.Id, cancellationToken);
        community.SetPolicyType(request.PolicyType);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
