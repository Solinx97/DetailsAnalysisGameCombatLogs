using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.CombatLogIsReady;

internal class CombatLogIsReadyHandler(ICombatLogRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CombatLogIsReadyCommand>
{
    private readonly ICombatLogRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CombatLogIsReadyCommand request, CancellationToken cancellationToken)
    {
        var combatLog = await _repository.GetByIdAsync(request.Id, cancellationToken);

        combatLog.CombatLogIsReady(request.NumberReadyCombats, request.CombatsInQueue);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
