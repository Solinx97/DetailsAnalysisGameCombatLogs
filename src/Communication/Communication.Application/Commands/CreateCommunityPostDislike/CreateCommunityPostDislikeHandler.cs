using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostDislike;

internal class CreateCommunityPostDislikeHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCommunityPostDislikeCommand, CommunityPostDislikeDto>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityPostDislikeDto> Handle(CreateCommunityPostDislikeCommand request, CancellationToken cancelationToken)
    {
        var communityPost = await _repository.GetWithReactionsAsync(request.CommunityPostId, cancelationToken);
        var (dislike, status) = communityPost.AddDislike(request.CommunityId, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<CommunityPostDislikeDto>(dislike);
        map.Status = (int)status;

        return map;
    }
}
