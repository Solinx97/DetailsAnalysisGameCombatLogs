using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunity;

internal class CreateCommunityHandler(IGenericRepository<Community, int> repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCommunityCommand, CommunityDto>
{
    private readonly IGenericRepository<Community, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityDto> Handle(CreateCommunityCommand request, CancellationToken cancelationToken)
    {
        var community = Community.Create(request.Name, request.Description, request.PolicyType, request.AppUserId);
        await _repository.AddAsync(community, cancelationToken);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<CommunityDto>(community);

        return map;
    }
}
