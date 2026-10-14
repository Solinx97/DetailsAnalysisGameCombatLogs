using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityUser;

internal class CreateCommunityUserHandler(IGenericRepository<Community, CommunityId> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunityUserCommand>
{
    private readonly IGenericRepository<Community, CommunityId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateCommunityUserCommand request, CancellationToken cancelationToken)
    {
        var community = await _repository.GetByIdAsync(request.CommunityId, cancelationToken);
        community.AddMember(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}

