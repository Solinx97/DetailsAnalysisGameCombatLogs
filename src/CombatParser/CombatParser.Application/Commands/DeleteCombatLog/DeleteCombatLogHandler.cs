using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.DeleteCombatLog;

internal class DeleteCombatLogHandler(ICombatLogRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCombatLogCommand>
{
    private readonly ICombatLogRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCombatLogCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
