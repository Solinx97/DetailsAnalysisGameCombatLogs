using Chat.Application.Commands.PersonalChat.CreateMessage;
using Chat.Application.Interfaces;
using Chat.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChatApplication(this IServiceCollection services)
    {
        services.AddScoped<IGroupChatUserService, GroupChatUserService>();
        services.AddScoped<IVoiceChatService, VoiceChatService>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateMessageCommand).Assembly));
    }
}
