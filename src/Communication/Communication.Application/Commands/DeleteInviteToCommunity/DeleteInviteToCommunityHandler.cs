using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteInviteToCommunity;

internal class DeleteInviteToCommunityHandler(ICommunityRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteInviteToCommunityCommand>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteInviteToCommunityCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetWithInvitesAsync(request.CommunityId, cancellationToken);
        community.RemoveInvite(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

