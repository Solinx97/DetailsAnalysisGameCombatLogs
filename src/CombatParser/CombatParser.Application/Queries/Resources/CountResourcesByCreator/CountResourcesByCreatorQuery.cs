using MediatR;

namespace CombatParser.Application.Queries.Resources.CountResourcesByCreator;

public record CountResourcesByCreatorQuery(
    int CombatPlayerId,
    string Target
    ) : IRequest<int>;
