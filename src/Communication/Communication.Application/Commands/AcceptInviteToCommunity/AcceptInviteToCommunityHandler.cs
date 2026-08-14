using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.AcceptInviteToCommunity;

internal class AcceptInviteToCommunityHandler(ICommunityRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<AcceptInviteToCommunityCommand>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AcceptInviteToCommunityCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetWithInvitesAndUsersAsync(request.CommunityId, cancellationToken);
        community.AcceptInvite(request.Id, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
