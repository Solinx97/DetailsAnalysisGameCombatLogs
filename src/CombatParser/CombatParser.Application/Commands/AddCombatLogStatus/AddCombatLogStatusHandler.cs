using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.AddCombatLogStatus;

internal class AddCombatLogStatusHandler(IGenericRepository<CombatLog, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<AddCombatLogStatusCommand>
{
    private readonly IGenericRepository<CombatLog, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AddCombatLogStatusCommand request, CancellationToken cancellationToken)
    {
        var combatLog = await _repository.GetByIdAsync(request.CombatLogId, cancellationToken);
        combatLog.AddStatus(request.Status);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
