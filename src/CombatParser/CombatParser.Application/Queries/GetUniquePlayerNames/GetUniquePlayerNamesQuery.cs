using MediatR;

namespace CombatParser.Application.Queries.GetUniquePlayerNames;

public record GetUniquePlayerNamesQuery(
    int CombatLogId
    ) : IRequest<IEnumerable<string>>;
