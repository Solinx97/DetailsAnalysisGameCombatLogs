﻿using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatBL.Services;
using CombatAnalysis.ChatBL.Services.Chat;
using CombatAnalysis.ChatDAL.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.ChatBL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ChatBLDependencies(this IServiceCollection services, string databaseName, string dataProcessingType, string connectionString)
    {
        services.RegisterDependenciesForDAL(databaseName, dataProcessingType, connectionString);

        services.AddScoped<IChatTransactionService, ChatTransactionService>();

        services.AddScoped<IService<VoiceChatDto, string>, VoiceChatService>();
        services.AddScoped<IService<PersonalChatDto, int>, PersonalChatService>();
        services.AddScoped<IChatMessageService<PersonalChatMessageDto, int>, PersonalChatMessageService>();
        services.AddScoped<IService<PersonalChatMessageCountDto, int>, PersonalChatMessageCountService>();
        services.AddScoped<IService<GroupChatDto, int>, GroupChatService>();
        services.AddScoped<IService<GroupChatRulesDto, int>, GroupChatRulesService>();
        services.AddScoped<IChatMessageService<GroupChatMessageDto, int>, GroupChatMessageService>();
        services.AddScoped<IService<UnreadGroupChatMessageDto, int>, UnreadGroupChatMessageService>();
        services.AddScoped<IService<GroupChatMessageCountDto, int>, GroupChatMessageCountService>();
        services.AddScoped<IServiceTransaction<GroupChatUserDto, string>, GroupChatUserService>();
    }
}
