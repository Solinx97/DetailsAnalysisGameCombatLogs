//using Chat.Application.Consts;
//using Chat.Application.DTOs;
//using Chat.Application.Enums;
//using Chat.Application.Interfaces;
//using Chat.Application.Kafka.Actions;
//using Chat.Application.Security;
//using Chat.Infrastructure.Persistence;
//using CombatAnalysis.ChatAPI.Consts;
//using CombatAnalysis.ChatAPI.Interfaces;
//using Confluent.Kafka;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using System.Text.Json;

//namespace CombatAnalysis.ChatAPI.Kafka;

//public class GroupChatConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs,  ILogger<GroupChatConsumer> logger,
//    IServiceScopeFactory serviceScopeFactory, IChatHubHelper groupChatHelper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChat, logger)
//{
//    private readonly Hubs _hubs = hubs.Value;
//    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
//    private readonly ILogger<GroupChatConsumer> _logger = logger;
//    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
//    private readonly IChatHubHelper _groupChatHelper = groupChatHelper;

//    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
//    {
//        try
//        {
//            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

//            using var scope = _serviceScopeFactory.CreateScope();

//            var dbContext = scope.ServiceProvider.GetRequiredService<ChatContext>();
//            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

//            await ExecuteAsync(scope, dbContext, kafkaData);
//        }
//        catch (ArgumentNullException ex)
//        {
//            _logger.LogError(ex, "Consume Group Chat data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChat, ex.ParamName);
//        }
//    }

//    private async Task ExecuteAsync(IServiceScope scope, ChatContext dbContext, ConsumeResult<string, JsonDocument> kafkaData)
//    {
//        await using var transaction = await dbContext.Database.BeginTransactionAsync();

//        try
//        {
//            var action = kafkaData.Message.Value.Deserialize<GroupChatAction>();
//            ArgumentNullException.ThrowIfNull(action, nameof(action));
//            ArgumentNullException.ThrowIfNull(action.Rules, nameof(action.Rules));
//            ArgumentNullException.ThrowIfNull(action.User, nameof(action.User));

//            switch(action.State)
//            {
//                case ChatActionState.Created:
//                    var chatId = await CreateChatRefsAsync(scope, action.Chat, action.Rules, action.User);

//                    await transaction.CommitAsync();

//                    await SendSignalRequestChatsAsync(action, chatId);

//                    break;
//                case ChatActionState.Removed:
//                    await RemoveChatRefsAsync(scope, action.Chat.Id);

//                    await transaction.CommitAsync();

//                    break;
//            }
//        }
//        catch (ArgumentNullException ex)
//        {
//            await transaction.RollbackAsync();

//            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChat, ex.ParamName);
//        }
//        catch (DbUpdateException ex)
//        {
//            await transaction.RollbackAsync();

//            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed.", KafkaTopics.GroupChat);
//        }
//        catch (Exception)
//        {
//            await transaction.RollbackAsync();

//            throw;
//        }
//    }

//    private static async Task<int> CreateChatRefsAsync(IServiceScope scope, GroupChatDto chat, GroupChatRulesDto chatRules, GroupChatUserDto chatUser)
//    {
//        var chatService = scope.ServiceProvider.GetService<IGroupChatService>();
//        ArgumentNullException.ThrowIfNull(chatService, nameof(chatService));

//        var chatUserService = scope.ServiceProvider.GetService<IGroupChatUserService>();
//        ArgumentNullException.ThrowIfNull(chatUserService, nameof(chatUserService));

//        var createdChat = await chatService.CreateAsync(chat);

//        chatRules.GroupChatId = createdChat.Id;

//        await chatService.AddRulesAsync(chatRules);

//        chatUser.Id = Guid.NewGuid().ToString();
//        chatUser.GroupChatId = createdChat.Id;

//        await chatUserService.CreateAsync(chatUser);

//        return createdChat.Id;
//    }

//    private static async Task RemoveChatRefsAsync(IServiceScope scope, int chatId)
//    {
//        var chatService = scope.ServiceProvider.GetService<IGroupChatService>();
//        ArgumentNullException.ThrowIfNull(chatService, nameof(chatService));

//        await chatService.DeleteAsync(chatId);
//    }

//    private async Task SendSignalRequestChatsAsync(GroupChatAction chatAction, int chatId)
//    {
//        var accessToken = AesEncryption.Decrypt(chatAction.AccessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));

//        await _groupChatHelper.ConnectToHubAsync(_hubs.GroupChatAddress, accessToken);

//        await _groupChatHelper.JoinRoomAsync(chatId);
//        await _groupChatHelper.RequestsChatsAsync(chatId, chatAction.User.AppUserId);

//        await _groupChatHelper.DisconnectFromHubAsync();
//    }
//}
