using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using MediatR;

namespace Communication.Application.Commands.CreateInviteToCommunity;

internal class CreateInviteToCommunityHandler(IGenericRepository<Community, CommunityId> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateInviteToCommunityCommand>
{
    private readonly IGenericRepository<Community, CommunityId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateInviteToCommunityCommand request, CancellationToken cancelationToken)
    {
        var community = await _repository.GetByIdAsync(request.CommunityId, cancelationToken);
        community.CreateInvite(request.AppUserId, request.ToAppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
