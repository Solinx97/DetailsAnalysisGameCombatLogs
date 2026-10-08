//using Chat.Application.Consts;
//using Chat.Application.Kafka.Actions;
//using Chat.Application.Security;
//using CombatAnalysis.NotificationAPI.Consts;
//using CombatAnalysis.NotificationAPI.Enums;
//using CombatAnalysis.NotificationAPI.Interfaces;
//using CombatAnalysis.NotificationBL.DTO;
//using CombatAnalysis.NotificationBL.Interfaces;
//using Confluent.Kafka;
//using Microsoft.Extensions.Options;
//using System.Text.Json;

//namespace CombatAnalysis.NotificationAPI.Kafka;

//public class PersonalChatMessageNotificationConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<PersonalChatMessageNotificationConsumer> logger,
//    IServiceScopeFactory serviceScopeFactory) : KafkaConsumerBase(kafkaSettings, KafkaTopics.PersonalChatMessage, logger)
//{
//    private readonly IOptions<Hubs> _hubs = hubs;
//    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
//    private readonly ILogger<PersonalChatMessageNotificationConsumer> _logger = logger;
//    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

//    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
//    {
//        try
//        {
//            using var scope = _serviceScopeFactory.CreateScope();

//            var notificationService = scope.ServiceProvider.GetService<IService<NotificationDto, int>>();
//            ArgumentNullException.ThrowIfNull(notificationService, nameof(notificationService));

//            var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
//            ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

//            var chatAction = kafkaData.Message.Value.Deserialize<PersonalChatMessageAction>();
//            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

//            var accessToken = AesEncryption.Decrypt(chatAction.AccessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));

//            await chatHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.NotificationAddress}", accessToken);
//            await chatHubHelper.JoinRoomAsync(chatAction.ChatMessage.AppUserId);

//            if (chatAction.State == (int)ChatActionState.Created)
//            {
//                await CreateNotificationAsync(chatHubHelper, chatAction, notificationService);
//            }

//            await chatHubHelper.DisconnectFromHubAsync();
//        }
//        catch (ArgumentNullException ex)
//        {
//            _logger.LogError(ex, "Consume Notification API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
//        }
//    }

//    private static async Task CreateNotificationAsync(IChatHubHelper chatHubHelper, PersonalChatMessageAction chatAction, IService<NotificationDto, int> notificationService)
//    {
//        var similarNotifcation = await notificationService.GetByParamAsync(n => n.InitiatorId, chatAction.ChatMessage.PersonalChatId.ToString());
//        var notifcationExist = similarNotifcation.Any(n => n.RecipientId == chatAction.RecipientId
//                                                        && n.Type == (int)NotificationType.PersonalChatMessage
//                                                        && n.Status == (int)NotificationStatus.Unread);

//        if (notifcationExist)
//        {
//            return;
//        }

//        var notification = new NotificationDto
//        {
//            InitiatorId = chatAction.ChatMessage.PersonalChatId.ToString(),
//            InitiatorName = chatAction.ChatMessage.Username,
//            RecipientId = chatAction.RecipientId,
//            Type = (int)NotificationType.PersonalChatMessage,
//            Status = (int)NotificationStatus.Unread,
//            CreatedAt = DateTime.UtcNow,
//        };

//        var createdNotification = await notificationService.CreateAsync(notification);
//        ArgumentNullException.ThrowIfNull(createdNotification, nameof(createdNotification));

//        await chatHubHelper.RequestNotificationAsync(createdNotification.Id, chatAction.ChatMessage.PersonalChatId);
//    }
//}
