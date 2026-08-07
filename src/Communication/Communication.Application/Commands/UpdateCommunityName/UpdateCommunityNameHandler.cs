using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.UpdateCommunityName;

internal class UpdateCommunityNameHandler(IGenericRepository<Community, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommunityNameCommand>
{
    private readonly IGenericRepository<Community, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommunityNameCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetByIdAsync(request.Id, cancellationToken);

        community.EditName(request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}