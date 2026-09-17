using CombatParser.Application.Commands.DeleteCombatLog;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface IDeleteCombatLogQueue
{
    ValueTask EnqueueAsync(DeleteCombatLogCommand job, CancellationToken cancellationToken = default);

    ValueTask<DeleteCombatLogCommand> DequeueAsync(CancellationToken cancellationToken);
}
