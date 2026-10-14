using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPost;

internal class CreateCommunityPostHandler(IGenericRepository<CommunityPost, CommunityPostId> repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCommunityPostCommand, CommunityPostDto>
{
    private readonly IGenericRepository<CommunityPost, CommunityPostId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityPostDto> Handle(CreateCommunityPostCommand request, CancellationToken cancelationToken)
    {
        var post = CommunityPost.Create(request.Content, request.PostType, request.PublicType, request.Restrictions, request.Tags, request.CommunityId, request.AppUserId);
        await _repository.AddAsync(post, cancelationToken);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<CommunityPostDto>(post);

        return map;
    }
}
