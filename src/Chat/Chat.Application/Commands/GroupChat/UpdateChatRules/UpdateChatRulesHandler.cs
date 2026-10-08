using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.GroupChat.UpdateChatRules;

internal class UpdateChatRulesHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateChatRulesCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateChatRulesCommand request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.ChatId);
        chat.UpdateRules(request.InvitePeople, request.RemovePeople, request.PinMessage, request.Announcements);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
