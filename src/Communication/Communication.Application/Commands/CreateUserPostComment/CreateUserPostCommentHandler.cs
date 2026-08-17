using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostComment;

internal class CreateUserPostCommentHandler(IGenericRepository<UserPost, int> repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateUserPostCommentCommand, UserPostCommentDto>
{
    private readonly IGenericRepository<UserPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UserPostCommentDto> Handle(CreateUserPostCommentCommand request, CancellationToken cancelationToken)
    {
        var userPost = await _repository.GetByIdAsync(request.UserPostId, cancelationToken);
        var comment = userPost.AddComment(request.Content, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<UserPostCommentDto>(comment);

        return map;
    }
}

