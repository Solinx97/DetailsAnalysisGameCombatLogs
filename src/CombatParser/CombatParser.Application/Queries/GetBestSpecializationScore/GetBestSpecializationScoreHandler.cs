using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetBestSpecializationScore;

internal class GetBestSpecializationScoreHandler(IBestSpecializationScoreRepository repository, IMapper mapper) : IRequestHandler<GetBestSpecializationScoreQuery, BestSpecializationScoreDto>
{
    private readonly IBestSpecializationScoreRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BestSpecializationScoreDto> Handle(GetBestSpecializationScoreQuery request, CancellationToken cancellationToken)
    {
        var specializationScore = await _repository.GetAsync(request.SpecId, request.BossId, cancellationToken);
        var map = _mapper.Map<BestSpecializationScoreDto>(specializationScore);

        return map;
    }
}
