using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using MediatR;

namespace CombatParser.Application.Commands.CreatePlayer;

internal class CreatePlayerHandler(IGenericRepository<Player, string> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreatePlayerCommand, Player>
{
    private readonly IGenericRepository<Player, string> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Player> Handle(CreatePlayerCommand request, CancellationToken cancelationToken)
    {
        var player = Player.Create(request.GameId, request.Username, request.Faction);
        await _repository.AddAsync(player, cancelationToken);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        return player;
    }
}
