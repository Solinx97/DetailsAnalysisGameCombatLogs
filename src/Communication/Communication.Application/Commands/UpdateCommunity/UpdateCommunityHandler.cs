using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using MediatR;

namespace Communication.Application.Commands.UpdateCommunity;

internal class UpdateCommunityHandler(IGenericRepository<Community, CommunityId> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommunityCommand>
{
    private readonly IGenericRepository<Community, CommunityId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetByIdAsync(request.Id, cancellationToken);
        community.Edit(request.Name, request.Description);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}