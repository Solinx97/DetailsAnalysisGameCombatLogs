using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostLike;

internal class CreateCommunityPostLikeHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCommunityPostLikeCommand, CommunityPostLikeDto>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityPostLikeDto> Handle(CreateCommunityPostLikeCommand request, CancellationToken cancelationToken)
    {
        var communityPost = await _repository.GetWithReactionsAsync(request.CommunityPostId, cancelationToken);
        var (like, status) = communityPost.AddLike(request.CommunityId, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<CommunityPostLikeDto>(like);
        map.Status = (int)status;

        return map;
    }
}
