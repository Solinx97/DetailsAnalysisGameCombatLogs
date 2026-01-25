using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.UpdateCombatLog;

internal class UpdateCombatLogHandler(ICombatLogRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCombatLogCommand>
{
    private readonly ICombatLogRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCombatLogCommand request, CancellationToken cancellationToken)
    {
        var combatLog = await _repository.GetByIdAsync(request.Id, cancellationToken);

        combatLog.Edit(request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}