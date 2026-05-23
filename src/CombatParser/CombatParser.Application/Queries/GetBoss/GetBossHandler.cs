using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetBoss;

internal class GetBossHandler(IBossRepository repository, IMapper mapper) : IRequestHandler<GetBossQuery, BossDto>
{
    private readonly IBossRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BossDto> Handle(GetBossQuery request, CancellationToken cancellationToken)
    {
        var boss = await _repository.GetAsync(request.GameBossId, request.Difficult, request.GroupSize, cancellationToken);
        var map = _mapper.Map<BossDto>(boss);

        return map;
    }
}