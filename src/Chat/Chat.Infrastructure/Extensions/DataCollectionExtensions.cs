using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Persistence;
using Chat.Infrastructure.Persistence.Outbox;
using Chat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.Extensions;

public static class DataCollectionExtensions
{
    public static void AddChatInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ChatContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IOutboxRepository, OutboxRepository>();

        services.AddScoped<IGenericRepository<PersonalChat, PersonalChatId>, GenericRepository<PersonalChat, PersonalChatId>>();
        services.AddScoped<IGenericRepository<PersonalChatMessage, PersonalChatMessageId>, GenericRepository<PersonalChatMessage, PersonalChatMessageId>>();
        services.AddScoped<IGenericRepository<GroupChatMessage, GroupChatMessageId>, GenericRepository<GroupChatMessage, GroupChatMessageId>>();
        services.AddScoped<IGenericRepository<GroupChat, GroupChatId>, GenericRepository<GroupChat, GroupChatId>>();
        services.AddScoped<IGenericRepository<GroupChatUser, GroupChatUserId>, GenericRepository<GroupChatUser, GroupChatUserId>>();
        services.AddScoped<IGenericRepository<VoiceChat, VoiceChatId>, GenericRepository<VoiceChat, VoiceChatId>>();
        services.AddScoped<IGroupChatMessageRepository, GroupChatMessageRepository>();
        services.AddScoped<IGroupChatRepository, GroupChatRepository>();
        services.AddScoped<IGroupChatUserRepository, GroupChatUserRepository>();
        services.AddScoped<IPersonalChatRepository, PersonalChatRepository>();
        services.AddScoped<IVoiceChatRepository, VoiceChatRepository>();
        services.AddScoped<IPersonalChatMessageRepository, PersonalChatMessageRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
