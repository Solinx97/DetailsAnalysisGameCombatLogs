using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.LeaveCommunityUser;

internal class LeaveCommunityUserHandler(ICommunityRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<LeaveCommunityUserCommand>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(LeaveCommunityUserCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetWithUsersAsync(request.CommunityId, cancellationToken);
        if (community.AppUserId == request.AppUserId)
        {
            await _repository.DeleteAsync(request.CommunityId, cancellationToken);
        }
        else
        {
            community.LeaveCommunity(request.AppUserId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
