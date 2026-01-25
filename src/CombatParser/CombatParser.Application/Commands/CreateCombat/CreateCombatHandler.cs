using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.CreateCombat;

internal class CreateCombatHandler(IGenericRepository<Combat, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCombatCommand, Combat>
{
    private readonly IGenericRepository<Combat, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Combat> Handle(CreateCombatCommand request, CancellationToken cancelationToken)
    {
        var combat = Combat.Create(request.DungeonName, request.BossHealthPercentage, request.DamageDone, request.HealDone, request.DamageTaken, 
            request.ResourcesRecovery, request.IsWin, request.StartDate, request.FinishDate, request.BossId, 
            request.CombatLogId, request.CombatPlayers);
        await _repository.AddAsync(combat, cancelationToken);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        return combat;
    }
}