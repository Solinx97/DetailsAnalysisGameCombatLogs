using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatParser.Application.Commands.DeleteCombatLog;
using System.Threading.Channels;

namespace CombatAnalysis.CombatParserAPI.Helpers;

public sealed class DeleteCombatLogQueue : IDeleteCombatLogQueue
{
    private readonly Channel<DeleteCombatLogCommand> _queue = Channel.CreateUnbounded<DeleteCombatLogCommand>();

    public ValueTask EnqueueAsync(DeleteCombatLogCommand job, CancellationToken cancellationToken = default)
    {
        return _queue.Writer.WriteAsync(job, cancellationToken);
    }

    public ValueTask<DeleteCombatLogCommand> DequeueAsync(CancellationToken cancellationToken)
    {
        return _queue.Reader.ReadAsync(cancellationToken);
    }
}
