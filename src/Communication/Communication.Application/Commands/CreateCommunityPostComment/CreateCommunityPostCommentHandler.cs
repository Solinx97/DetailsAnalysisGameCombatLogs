using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostComment;

internal class CreateCommunityPostCommentHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCommunityPostCommentCommand, CommunityPostCommentDto>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityPostCommentDto> Handle(CreateCommunityPostCommentCommand request, CancellationToken cancelationToken)
    {
        var post = await _repository.GetWithCommentsAsync(request.CommunityPostId, cancelationToken);
        var comment = post.AddComment(request.Content, request.CommentType, request.CommunityId, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<CommunityPostCommentDto>(comment);

        return map;
    }
}
