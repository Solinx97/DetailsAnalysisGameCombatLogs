using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.GroupChat.DeleteUser;

internal class DeleteUserHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.GroupChatId);
        chat.RemoveUser(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
