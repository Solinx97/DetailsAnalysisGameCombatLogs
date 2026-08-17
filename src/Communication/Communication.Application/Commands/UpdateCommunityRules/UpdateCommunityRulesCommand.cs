using MediatR;

namespace Communication.Application.Commands.UpdateCommunityRules;

public record UpdateCommunityRulesCommand(
    int Id,
    int PolicyType
    ) : IRequest;
