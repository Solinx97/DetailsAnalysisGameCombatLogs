using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetSpecializationBySpell;

public record GetSpecializationBySpellQuery(
    string Spells
    ) : IRequest<SpecializationDto>;
