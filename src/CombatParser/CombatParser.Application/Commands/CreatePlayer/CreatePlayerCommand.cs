using CombatParser.Domain.Entities;
using MediatR;

namespace CombatParser.Application.Commands.CreatePlayer;

public record CreatePlayerCommand(
    string GameId, 
    string Username, 
    int Faction
    ) : IRequest<Player>;
