using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetSpecializationBySpell;

internal class GetSpecializationBySpellHandler(ISpecializationRepository repository, IMapper mapper) : IRequestHandler<GetSpecializationBySpellQuery, SpecializationDto>
{
    private readonly ISpecializationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<SpecializationDto> Handle(GetSpecializationBySpellQuery request, CancellationToken cancellationToken)
    {
        var specialization = await _repository.GetBySpellsAsync(request.Spells, cancellationToken);
        var map = _mapper.Map<SpecializationDto>(specialization);

        return map;
    }
}