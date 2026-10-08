//using Chat.Application.Consts;
//using Chat.Application.DTOs;
//using Chat.Application.Enums;
//using Chat.Application.Interfaces;
//using Chat.Application.Kafka.Actions;
//using Chat.Application.Security;
//using Chat.Domain.Enums;
//using Chat.Infrastructure.Exceptions;
//using CombatAnalysis.ChatAPI.Consts;
//using CombatAnalysis.ChatAPI.Interfaces;
//using Confluent.Kafka;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using System.Text.Json;

//namespace CombatAnalysis.ChatAPI.Kafka;

//public class GroupChatMemberConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<GroupChatMemberConsumer> logger, IServiceScopeFactory serviceScopeFactory,
//    IKafkaProducerService<string, string> kafkaProducer, IChatHubHelper groupChatHelper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMember, logger)
//{
//    private readonly Hubs _hubs = hubs.Value;
//    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
//    private readonly ILogger<GroupChatMemberConsumer> _logger = logger;
//    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
//    private readonly IKafkaProducerService<string, string> _kafkaProducer = kafkaProducer;
//    private readonly IChatHubHelper _groupChatHelper = groupChatHelper;

//    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
//    {
//        try
//        {
//            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

//            using var scope = _serviceScopeFactory.CreateScope();

//            await ExecuteAsync(scope, kafkaData, stoppingToken);
//        }
//        catch (ArgumentNullException ex)
//        {
//            _logger.LogError(ex, "Consume Group Chat Member data (topic: {Topic}) failed: Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMember, ex.ParamName);
//        }
//    }

//    private async Task ExecuteAsync(IServiceScope scope, ConsumeResult<string, JsonDocument> kafkaData, CancellationToken cancellationToken)
//    {
//        try
//        {
//            var action = kafkaData.Message.Value.Deserialize<GroupChatMemberAction>();
//            ArgumentNullException.ThrowIfNull(action, nameof(action));
//            ArgumentNullException.ThrowIfNull(action.User, nameof(action.User));

//            var chatUserService = scope.ServiceProvider.GetService<IGroupChatUserService>();
//            ArgumentNullException.ThrowIfNull(chatUserService, nameof(chatUserService));

//            var chatOwnerUser = await chatUserService.FindByAppUserIdAsync(action.User.GroupChatId, action.ChatOwnerId);

//            switch (action.State)
//            {
//                case ChatMembersActionState.AddUser:
//                    await CreateGroupChatUser(chatUserService, action.User);

//                    await SendSignalRequestChatsAsync(action);
//                    await CreateSystemMessageAsync( $"Add user '{action.User.Username}' to chat", action, chatOwnerUser.Id, cancellationToken);

//                    break;
//                case ChatMembersActionState.RemoveUser:
//                    await RemoveGroupChatUser(chatUserService, action.User.Id);

//                    await CreateSystemMessageAsync($"Remove user '{action.User.Username}' from chat", action, chatOwnerUser.Id, cancellationToken);

//                    break;
//            }
//        }
//        catch (ArgumentNullException ex)
//        {
//            _logger.LogError(ex, "Create group chat user from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMember, ex.ParamName);
//        }
//        catch (EntityNotFoundException ex)
//        {
//            _logger.LogWarning("Delete group chat user from Kafka Consumer (topic: {Topic}) failed. Group chat user {Id} not found.", KafkaTopics.GroupChatMember, ex.EntityId);
//        }
//        catch (DbUpdateException ex)
//        {
//            _logger.LogError(ex, "Failed to create chat user from Kafka Consumer (topic: {Topic})", KafkaTopics.GroupChatMember);
//        }
//    }

//    private static async Task CreateGroupChatUser(IGroupChatUserService chatUserService, GroupChatUserDto chatUser)
//    {
//        await chatUserService.CreateAsync(chatUser);
//    }

//    private async Task SendSignalRequestChatsAsync(GroupChatMemberAction chatAction)
//    {
//        var accessToken = AesEncryption.Decrypt(chatAction.AccessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));

//        await _groupChatHelper.ConnectToHubAsync(_hubs.GroupChatAddress, accessToken);

//        await _groupChatHelper.JoinRoomAsync(chatAction.User.GroupChatId);
//        await _groupChatHelper.RequestsChatsAsync(chatAction.User.GroupChatId, chatAction.User.AppUserId);

//        await _groupChatHelper.DisconnectFromHubAsync();
//    }

//    private async Task CreateSystemMessageAsync(string systemMessage, GroupChatMemberAction chatAction, string chatOwnerUserId, CancellationToken cancellationToken)
//    {
//        var chatMessageAction = JsonSerializer.Serialize(new GroupChatMessageAction
//        {
//            ChatMessage = new GroupChatMessageDto
//            {
//                Id = 0,
//                Username = "System",
//                Message = systemMessage,
//                Time = chatAction.When,
//                Status = MessageStatus.Sent,
//                Type = MessageType.System,
//                MarkedType = MessageMarkedType.None,
//                IsEdited = false,
//                GroupChatId = chatAction.User.GroupChatId,
//                GroupChatUserId = chatOwnerUserId
//            },
//            State = (int)ChatMessageActionState.Created,
//            When = DateTimeOffset.UtcNow,
//            AccessToken = chatAction.AccessToken
//        });
//        await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMessage, Guid.NewGuid().ToString(), chatMessageAction, cancellationToken);
//    }

//    private static async Task RemoveGroupChatUser(IGroupChatUserService chatUserService, string chatUserId)
//    {
//        await chatUserService.DeleteAsync(chatUserId);
//    }
//}
