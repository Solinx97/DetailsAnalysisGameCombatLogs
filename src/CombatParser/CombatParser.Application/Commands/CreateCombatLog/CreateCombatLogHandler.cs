using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.CreateCombatLog;

internal class CreateCombatLogHandler(IGenericRepository<CombatLog, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCombatLogCommand, int>
{
    private readonly IGenericRepository<CombatLog, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int> Handle(CreateCombatLogCommand request, CancellationToken cancelationToken)
    {
        var combatLog = CombatLog.Create(request.GameVersion, request.Name, request.LogType, request.AppUserId);
        combatLog.AddStatus((int)Domain.Enums.CombatLogStatus.Creating);

        await _repository.AddAsync(combatLog, cancelationToken);
        await _unitOfWork.SaveChangesAsync(cancelationToken);

        return combatLog.Id;
    }
}