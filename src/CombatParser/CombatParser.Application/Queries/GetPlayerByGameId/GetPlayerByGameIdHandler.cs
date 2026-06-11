using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetPlayerByGameId;

internal class GetPlayerByGameIdHandler(IPlayerRepository repository, IMapper mapper) : IRequestHandler<GetPlayerByGameIdQuery, PlayerDto>
{
    private readonly IPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PlayerDto> Handle(GetPlayerByGameIdQuery request, CancellationToken cancellationToken)
    {
        var player = await _repository.GetByGameIdAsync(request.GameId, cancellationToken);
        var map = _mapper.Map<PlayerDto>(player);

        return map;
    }
}

