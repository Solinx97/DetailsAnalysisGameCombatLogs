using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetSpecializationScore;

internal class GetSpecializationScoreHandler(ICombatPlayerDataRepository<SpecializationScore> repository, IMapper mapper) : IRequestHandler<GetSpecializationScoreQuery, SpecializationScoreDto>
{
    private readonly ICombatPlayerDataRepository<SpecializationScore> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<SpecializationScoreDto> Handle(GetSpecializationScoreQuery request, CancellationToken cancellationToken)
    {
        var specializationScore = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<SpecializationScoreDto>(specializationScore);

        return map;
    }
}