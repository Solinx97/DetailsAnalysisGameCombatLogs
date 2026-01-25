using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.CreateCombat;

internal class CreateCombatHandler(ICombatRepository repository) : IRequestHandler<CreateCombatCommand, Combat>
{
    private readonly ICombatRepository _repository = repository;

    public async Task<Combat> Handle(CreateCombatCommand request, CancellationToken cancelationToken)
    {
        var combat = Combat.Create(request.DungeonName, request.BossHealthPercentage, request.DamageDone, request.HealDone, request.DamageTaken, 
            request.ResourcesRecovery, request.IsWin, request.StartDate, request.FinishDate, request.BossId, 
            request.CombatLogId, request.CombatPlayers, request.CombatAuras);

        await _repository.AddBulkAsync(combat, cancelationToken);

        return combat;
    }
}