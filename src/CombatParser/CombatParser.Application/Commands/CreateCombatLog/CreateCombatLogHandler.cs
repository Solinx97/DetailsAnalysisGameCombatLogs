using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.CreateCombatLog;

internal class CreateCombatLogHandler(IGenericRepository<CombatLog, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCombatLogCommand, CombatLog>
{
    private readonly IGenericRepository<CombatLog, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CombatLog> Handle(CreateCombatLogCommand request, CancellationToken cancelationToken)
    {
        var combatLog = CombatLog.Create(request.Name, request.LogType, request.NumberReadyCombats, request.CombatsInQueue, request.AppUserId);
        await _repository.AddAsync(combatLog, cancelationToken);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        return combatLog;
    }
}