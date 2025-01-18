using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class GroupChatMessagesViewModel : MvxViewModel, IImprovedMvxViewModel
{
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;

    private ObservableCollection<GroupChatMessageModel>? _messages;
    private IEnumerable<GroupChatMessageModel>? _allMessages;
    private ObservableCollection<AppUserModel>? _usersToInviteToChat;
    private ObservableCollection<AppUserModel>? _users;
    private List<AppUserModel>? _usersExcludingInvitees;
    private GroupChatModel? _selectedChat;
    private string _meInChatId = string.Empty;
    private GroupChatMessageModel? _selectedMessage;
    private int _selectedMessageIndex = -1;
    private string? _selectedChatName;
    private string? _message;
    private bool _chatMenuIsVisibly;
    private AppUserModel? _myAccount;
    private LoadingStatus _addUserToChatResponse;
    private int _selectedUsersForInviteToGroupChatIndex = -1;
    private string? _inputedUserEmailForInviteToChat;
    private bool _inviteToChatIsVisibly;
    private bool _isEditMode;
    private bool _isRemoveMode;
    private IChatHubHelper? _hubConnection;

    public GroupChatMessagesViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        Handler = new VMHandler<GroupChatMessagesViewModel>();
        Parent = this;
        SavedViewModel = this;

        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        SendMessageCommand = new MvxAsyncCommand(SendMessageAsync);
        ShowChatMenuCommand = new MvxCommand(() => ChatMenuIsVisibly = !ChatMenuIsVisibly);
        OpenInviteToChatCommand = new MvxAsyncCommand(OpenInviteToChatAsync);
        InviteToChatCommand = new MvxAsyncCommand(InviteToChatAsync);
        CloseInviteToChatCommand = new MvxCommand(SwitchInviteToGroupChat);
        TurnOnEditModeCommand = new MvxCommand(() => IsEditMode = !IsEditMode);
        EditMessageCommand = new MvxAsyncCommand(EditMessageAsync);
        RemoveMessageCommand = new MvxAsyncCommand(RemoveMessageAsync);

        Messages = [];
        _allMessages = [];

        Task.Run(LoadUsersAsync);
    }

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    #region Commands

    public IMvxAsyncCommand SendMessageCommand { get; set; }

    public IMvxCommand ShowChatMenuCommand { get; set; }

    public IMvxAsyncCommand OpenInviteToChatCommand { get; set; }

    public IMvxAsyncCommand InviteToChatCommand { get; set; }

    public IMvxCommand CloseInviteToChatCommand { get; set; }

    public IMvxCommand TurnOnEditModeCommand { get; set; }

    public IMvxAsyncCommand EditMessageCommand { get; set; }

    public IMvxAsyncCommand RemoveMessageCommand { get; set; }

    #endregion

    #region View model properties

    public string MeInChatId
    {
        get { return _meInChatId; }
        set
        {
            SetProperty(ref _meInChatId, value);
        }
    }

    public ObservableCollection<AppUserModel>? Users
    {
        get { return _users; }
        set
        {
            SetProperty(ref _users, value);
        }
    }

    public ObservableCollection<GroupChatMessageModel>? Messages
    {
        get { return _messages; }
        set
        {
            SetProperty(ref _messages, value);
        }
    }

    public GroupChatModel? SelectedChat
    {
        get { return _selectedChat; }
        set
        {
            SetProperty(ref _selectedChat, value);
        }
    }

    public GroupChatMessageModel? SelectedMessage
    {
        get { return _selectedMessage; }
        set
        {
            SetProperty(ref _selectedMessage, value);
        }
    }

    public int SelectedMessageIndex
    {
        get { return _selectedMessageIndex; }
        set
        {
            SetProperty(ref _selectedMessageIndex, value);
        }
    }

    public string? SelectedChatName
    {
        get { return _selectedChatName; }
        set
        {
            SetProperty(ref _selectedChatName, value);
        }
    }

    public string? Message
    {
        get { return _message; }
        set
        {
            SetProperty(ref _message, value);
        }
    }

    public bool ChatMenuIsVisibly
    {
        get { return _chatMenuIsVisibly; }
        set
        {
            SetProperty(ref _chatMenuIsVisibly, value);
        }
    }

    public AppUserModel? MyAccount
    {
        get { return _myAccount; }
        set
        {
            SetProperty(ref _myAccount, value);
        }
    }

    public LoadingStatus AddUserToChatResponse
    {
        get { return _addUserToChatResponse; }
        set
        {
            SetProperty(ref _addUserToChatResponse, value);
        }
    }

    public ObservableCollection<AppUserModel>? UsersToInviteToChat
    {
        get { return _usersToInviteToChat; }
        set
        {
            SetProperty(ref _usersToInviteToChat, value);
        }
    }

    public bool InviteToChatIsVisibly
    {
        get { return _inviteToChatIsVisibly; }
        set
        {
            SetProperty(ref _inviteToChatIsVisibly, value);
        }
    }

    public int SelectedUsersForInviteToGroupChatIndex
    {
        get { return _selectedUsersForInviteToGroupChatIndex; }
        set
        {
            SetProperty(ref _selectedUsersForInviteToGroupChatIndex, value);
        }
    }

    public string? InputedUserEmailForInviteToChat
    {
        get { return _inputedUserEmailForInviteToChat; }
        set
        {
            SetProperty(ref _inputedUserEmailForInviteToChat, value);
            if (value != null)
            {
                LoadUsernamesForInviteByStartChars(value);
            }
        }
    }

    public bool IsEditMode
    {
        get { return _isEditMode; }
        set
        {
            SetProperty(ref _isEditMode, value);
        }
    }

    public bool IsRemoveMode
    {
        get { return _isRemoveMode; }
        set
        {
            SetProperty(ref _isRemoveMode, value);
        }
    }

    #endregion

    public override void ViewDestroy(bool viewFinishing = true)
    {
        //if (_hubConnection != null)
        //{
        //    Task.Run(async () => await _hubConnection.SendAsync("LeaveFromRoom", SelectedChat?.Id.ToString()));
        //    Task.Run(async () => await _hubConnection.StopAsync());
        //}

        base.ViewDestroy(viewFinishing);
    }

    public async Task SendMessageHasBeenReadAsync(GroupChatMessageModel message)
    {
        try
        {
            if (_hubConnection == null)
            {
                throw new ArgumentNullException(nameof(_hubConnection));
            }
            else if (string.IsNullOrEmpty(MeInChatId))
            {
                throw new ArgumentNullException(nameof(MeInChatId));
            }

            await _hubConnection.SubscribeMessageHasBeenReadAsync(message.Id, MeInChatId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task InitChatSignalRAsync(IChatHubHelper hubConnection)
    {
        try
        {
            await GetMeInGroupChatAsync();
            await LoadMessagesForSelectedChatAsync(SelectedChat?.Name ?? string.Empty);

            _hubConnection = hubConnection;

            if (SelectedChat == null)
            {
                throw new ArgumentNullException(nameof(SelectedChat));
            }
            else if (string.IsNullOrEmpty(MeInChatId))
            {
                throw new ArgumentNullException(nameof(MeInChatId));
            }

            await hubConnection.ConnectToChatHubAsync($"{Hubs.Port}{Hubs.GroupChatAddress}");
            await hubConnection.JoinChatRoom(SelectedChat.Id);

            hubConnection.SubscribeMessagesUpdated<GroupChatMessageModel>(SelectedChat.Id, MeInChatId, async (message) =>
            {
                await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    Messages?.Insert(0, message);
                });
            });
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task FillAsync()
    {
        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            if (_allMessages == null || Messages == null)
            {
                return;
            }

            foreach (var item in _allMessages)
            {
                if (item.ChatId == SelectedChat?.Id
                    && !Messages.Any(x => x.Id == item.Id))
                {
                    Messages.Add(item);
                }
            }
        });
    }

    private async Task SendMessageAsync()
    {
        try
        {
            if (Message == null)
            {
                throw new ArgumentNullException(nameof(Message));
            }
            else if (SelectedChat == null)
            {
                throw new ArgumentNullException(nameof(SelectedChat));
            }
            else if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }
            else if (string.IsNullOrEmpty(MeInChatId))
            {
                throw new ArgumentNullException(nameof(MeInChatId));
            }
            else if (_hubConnection == null)
            {
                throw new ArgumentNullException(nameof(_hubConnection));
            }

            await _hubConnection.SendMessageAsync(Message, SelectedChat.Id, MeInChatId, MyAccount.Username);

            Message = string.Empty;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task OpenInviteToChatAsync()
    {
        AddUserToChatResponse = LoadingStatus.None;

        try
        {
            UsersToInviteToChat?.Clear();

            var response = await _httpClientHelper.GetAsync("GroupChatUser", Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var groupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
            if (groupChatUsers == null)
            {
                throw new ArgumentNullException(nameof(groupChatUsers));
            }

            _usersExcludingInvitees = Users?.Where(x => !groupChatUsers.Any(y => x.Id == y.AppUserId)).ToList();
            if (_usersExcludingInvitees == null)
            {
                throw new ArgumentNullException(nameof(_usersExcludingInvitees));
            }

            UsersToInviteToChat = new ObservableCollection<AppUserModel>(_usersExcludingInvitees);

            SwitchInviteToGroupChat();
        }
        catch (ArgumentNullException ex)
        {
            AddUserToChatResponse = LoadingStatus.Failed;

            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            AddUserToChatResponse = LoadingStatus.Failed;

            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            AddUserToChatResponse = LoadingStatus.Failed;

            _logger.LogError(ex, ex.Message);
        }
    }

    private void SwitchInviteToGroupChat()
    {
        InviteToChatIsVisibly = !InviteToChatIsVisibly;
    }

    private async Task InviteToChatAsync()
    {
        try
        {
            if (SelectedChat == null)
            {
                throw new ArgumentNullException(nameof(SelectedChat));
            }
            else if (UsersToInviteToChat == null)
            {
                throw new ArgumentNullException(nameof(UsersToInviteToChat));
            }

            var userId = UsersToInviteToChat[SelectedUsersForInviteToGroupChatIndex].Id;

            var groupChatUser = new GroupChatUserModel
            {
                ChatId = SelectedChat.Id,
                AppUserId = userId,
            };

            InputedUserEmailForInviteToChat = string.Empty;

            AddUserToChatResponse = LoadingStatus.Pending;

            var response = await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser), CancellationToken.None);
            response.EnsureSuccessStatusCode();

            SwitchInviteToGroupChat();

            AddUserToChatResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            AddUserToChatResponse = LoadingStatus.Failed;

            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            AddUserToChatResponse = LoadingStatus.Failed;

            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            AddUserToChatResponse = LoadingStatus.Failed;

            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task EditMessageAsync()
    {
        try
        {
            var response = await _httpClientHelper.PutAsync("GroupChatMessage", JsonContent.Create(SelectedMessage), Port.ChatApi);
            response.EnsureSuccessStatusCode();

            IsEditMode = false;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task RemoveMessageAsync()
    {
        try
        {
            var response = await _httpClientHelper.DeletAsync($"GroupChatMessage/{SelectedMessage?.Id}", Port.ChatApi);
            response.EnsureSuccessStatusCode();

            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                Messages?.Remove(Messages[SelectedMessageIndex]);
                SelectedMessage = null;
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task LoadMessagesForSelectedChatAsync(string chatName)
    {
        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            Messages?.Clear();
        });

        SelectedChatName = chatName;

        await LoadMessagesAsync();
    }

    private async Task LoadMessagesAsync()
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.GetAsync($"GroupChatMessage/getByChatId?chatId={SelectedChat?.Id}&pageSize=20", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            _allMessages = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();
            if (_allMessages == null)
            {
                throw new ArgumentNullException(nameof(_allMessages));
            }

            await FillAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            var response = await _httpClientHelper.GetAsync("Account", Port.UserApi);
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            Users = new ObservableCollection<AppUserModel>(users.Where(x => x.Id != MyAccount?.Id).ToList());
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void LoadUsernamesForInviteByStartChars(string startChars)
    {
        if (!string.IsNullOrEmpty(startChars))
        {
            var usersEmailByStartChars = Users?.Where(x => x.Username.StartsWith(startChars));
            if (usersEmailByStartChars == null)
            {
                return;
            }

            UsersToInviteToChat = new ObservableCollection<AppUserModel>(usersEmailByStartChars);
        }
        else
        {
            UsersToInviteToChat = new ObservableCollection<AppUserModel>(_usersExcludingInvitees ?? new List<AppUserModel>());
        }
    }

    private async Task GetMeInGroupChatAsync()
    {
        try
        {
            MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }
            else if (SelectedChat == null)
            {
                throw new ArgumentNullException(nameof(SelectedChat));
            }

            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.GetAsync($"GroupChatUser/findMeInChat?chatId={SelectedChat.Id}&appUserId={MyAccount.Id}", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var meInChat = await response.Content.ReadFromJsonAsync<GroupChatUserModel>();
            if (meInChat == null)
            {
                throw new ArgumentNullException(nameof(meInChat));
            }

            MeInChatId = meInChat.Id;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
