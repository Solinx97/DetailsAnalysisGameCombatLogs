using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteCommunityUser;

internal class DeleteCommunityUserHandler(ICommunityRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommunityUserCommand>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCommunityUserCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetWithCommunityUsersAsync(request.CommunityId, cancellationToken);
        community.RemoveMember(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
