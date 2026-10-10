using Chat.Application.Commands.PersonalChat.CreateMessage;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChatApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateMessageCommand).Assembly));
    }
}
