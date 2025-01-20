using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.Containers;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class ChatViewModel : ParentTemplate
{
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly ILogger _logger;
    private readonly IMemoryCache _memoryCache;

    private bool _isChatSelected;
    private IImprovedMvxViewModel? _personalChatMessagesTemplate;
    private IImprovedMvxViewModel? _groupChatMessagesTemplate;
    private ObservableCollection<MyGroupChatContainerModel>? _myGroupChats;
    private ObservableCollection<MyPersonalChatContainerModel>? _personalChats;
    private ObservableCollection<AppUserModel>? _users;
    private List<AppUserModel>? _allUsers;
    private string? _inputedUsername;
    private int _selectedUsersIndex = -1;
    private MyGroupChatContainerModel? _selectedMyGroupChat;
    private MyPersonalChatContainerModel? _selectedPersonalChat;
    private AppUserModel? _myAccount;
    private LoadingStatus _groupChatLoadingResponse;
    private LoadingStatus _personalChatLoadingResponse;
    private IChatHubHelper _personalChatHubConnection;
    private IChatHubHelper _groupChatHubConnection;

    public ChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger, 
        IChatHubHelper personalChatHubConnection, IChatHubHelper groupChatHubConnection)
    {
        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;
        _personalChatHubConnection = personalChatHubConnection;
        _groupChatHubConnection = groupChatHubConnection;

        RefreshGroupChatsCommand = new MvxAsyncCommand(LoadGroupChatsAsync);
        RefreshPersonalChatsCommand = new MvxAsyncCommand(LoadPersonalChatsAsync);
        CreatePersonalChatCommand = new MvxAsyncCommand(CreateNewPersonalChatAsync);

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), -2);

        GetMyAccount();
    }

    #region Commands

    public IMvxAsyncCommand RefreshGroupChatsCommand { get; set; }

    public IMvxAsyncCommand CreatePersonalChatCommand { get; set; }

    public IMvxAsyncCommand RefreshPersonalChatsCommand { get; set; }

    #endregion

    #region View model properties

    public bool IsChatSelected
    {
        get { return _isChatSelected; }

        set
        {
            SetProperty(ref _isChatSelected, value);
        }
    }

    public IImprovedMvxViewModel? PersonalChatMessagesTemplate
    {
        get { return _personalChatMessagesTemplate; }

        set
        {
            SetProperty(ref _personalChatMessagesTemplate, value);
        }
    }

    public IImprovedMvxViewModel? GroupChatMessagesTemplate
    {
        get { return _groupChatMessagesTemplate; }

        set
        {
            SetProperty(ref _groupChatMessagesTemplate, value);
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

    public ObservableCollection<MyGroupChatContainerModel>? MyGroupChats
    {
        get { return _myGroupChats; }
        set
        {
            SetProperty(ref _myGroupChats, value);
        }
    }

    public ObservableCollection<MyPersonalChatContainerModel>? MyPersonalChats
    {
        get { return _personalChats; }
        set
        {
            SetProperty(ref _personalChats, value);
        }
    }

    public int SelectedUsersIndex
    {
        get { return _selectedUsersIndex; }
        set
        {
            SetProperty(ref _selectedUsersIndex, value);
        }
    }

    public string? InputedUsername
    {
        get { return _inputedUsername; }
        set
        {
            SetProperty(ref _inputedUsername, value);
            if (!string.IsNullOrEmpty(value))
            {
                LoadAppUserUsernameByStartChars(value);
            }
        }
    }

    public MyGroupChatContainerModel? SelectedMyGroupChat
    {
        get { return _selectedMyGroupChat; }
        set
        {
            SetProperty(ref _selectedMyGroupChat, value);

            if (value != null && value.GroupChat != null)
            {
                IsChatSelected = true;
                SelectedPersonalChat = null;

                PersonalChatMessagesTemplate?.ViewDestroy();
                PersonalChatMessagesTemplate = null;

                GroupChatMessagesTemplate = Mvx.IoCProvider?.IoCConstruct<GroupChatMessagesViewModel>();
                GroupChatMessagesTemplate?.Handler.PropertyUpdate<GroupChatMessagesViewModel>(GroupChatMessagesTemplate, nameof(GroupChatMessagesViewModel.SelectedChat), value.GroupChat);

                var groupChatMessagesVewModel = GroupChatMessagesTemplate as GroupChatMessagesViewModel;
                if (groupChatMessagesVewModel != null)
                {
                    Task.Run(async () => await groupChatMessagesVewModel.InitChatSignalRAsync(_groupChatHubConnection));
                }
            }
        }
    }

    public MyPersonalChatContainerModel? SelectedPersonalChat
    {
        get { return _selectedPersonalChat; }
        set
        {
            SetProperty(ref _selectedPersonalChat, value);

            if (value != null && value.PersonalChat != null)
            {
                IsChatSelected = true;
                SelectedMyGroupChat = null;

                GroupChatMessagesTemplate?.ViewDestroy();
                GroupChatMessagesTemplate = null;

                PersonalChatMessagesTemplate = Mvx.IoCProvider?.IoCConstruct<PersonalChatMessagesVewModel>();
                PersonalChatMessagesTemplate?.Handler.PropertyUpdate<PersonalChatMessagesVewModel>(PersonalChatMessagesTemplate, nameof(PersonalChatMessagesVewModel.SelectedChat), value.PersonalChat);
                
                var personalChatMessagesVewModel = PersonalChatMessagesTemplate as PersonalChatMessagesVewModel;
                if (personalChatMessagesVewModel != null)
                {
                    Task.Run(async () => await personalChatMessagesVewModel.InitChatSignalRAsync(_personalChatHubConnection));
                }
            }
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

    public LoadingStatus GroupChatLoadingResponse
    {
        get { return _groupChatLoadingResponse; }
        set
        {
            SetProperty(ref _groupChatLoadingResponse, value);

            RaisePropertyChanged(() => IsShowEmptyMyGroupChat);
            RaisePropertyChanged(() => IsLoadMyGroupChatList);
        }
    }

    public LoadingStatus PersonalChatLoadingResponse
    {
        get { return _personalChatLoadingResponse; }
        set
        {
            SetProperty(ref _personalChatLoadingResponse, value);

            RaisePropertyChanged(() => IsShowEmptyPersonalChat);
            RaisePropertyChanged(() => IsLoadPersonalChatList);
        }
    }

    public bool IsShowEmptyMyGroupChat
    {
        get
        {
            return (int)GroupChatLoadingResponse > 0 && (int)GroupChatLoadingResponse < 3
                        && MyGroupChats?.Count == 0;
        }
    }

    public bool IsLoadMyGroupChatList
    {
        get
        {
            return (int)GroupChatLoadingResponse > 0 && (int)GroupChatLoadingResponse < 3
                        && MyGroupChats?.Count > 0;
        }
    }

    public bool IsShowEmptyPersonalChat
    {
        get
        {
            return (int)PersonalChatLoadingResponse > 0 && (int)PersonalChatLoadingResponse < 3
                        && MyPersonalChats?.Count == 0;
        }
    }

    public bool IsLoadPersonalChatList
    {
        get
        {
            return (int)PersonalChatLoadingResponse > 0 && (int)PersonalChatLoadingResponse < 3
                        && MyPersonalChats?.Count > 0;
        }
    }

    #endregion

    public override void Prepare()
    {
        base.Prepare();

        Task.Run(InitChatSignalRAsync);

        Task.Run(LoadGroupChatsAsync);
        Task.Run(LoadPersonalChatsAsync);
    }

    private async Task CreateNewPersonalChatAsync()
    {
        try
        {
            if (Users == null)
            {
                throw new ArgumentNullException(nameof(Users));
            }
            else if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }

            var targetUser = Users[SelectedUsersIndex];
            var personalChat = new PersonalChatModel
            {
                InitiatorId = MyAccount.Id,
                CompanionId = targetUser.Id,
            };

            InputedUsername = string.Empty;

            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.PostAsync("PersonalChat", JsonContent.Create(personalChat), refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            await LoadPersonalChatsAsync();
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

    private async Task LoadGroupChatsAsync()
    {
        GroupChatLoadingResponse = LoadingStatus.Pending;

        IsChatSelected = false;

        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.GetAsync($"GroupChatUser/findByUserId/{MyAccount?.Id}", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var myGroupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
            if (myGroupChatUsers == null)
            {
                throw new ArgumentNullException(nameof(myGroupChatUsers));
            }

            await GetMyGroupChatsByUserIdAsync(myGroupChatUsers, refreshToken);

            GroupChatLoadingResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
    }

    private async Task LoadPersonalChatsAsync()
    {
        PersonalChatLoadingResponse = LoadingStatus.Pending;

        IsChatSelected = false;

        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            var response = await _httpClientHelper.GetAsync("PersonalChat", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var personalChats = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
            var myPersonalChats = personalChats?
                .Where(x => x.InitiatorId == MyAccount?.Id || x.CompanionId == MyAccount?.Id)
                .ToList();
            if (myPersonalChats == null)
            {
                throw new ArgumentNullException(nameof(myPersonalChats));
            }

            await CreatePersonalChatContainerAsync(myPersonalChats, refreshToken);
            await LoadUsersAsync();

            PersonalChatLoadingResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
    }

    private async Task GetMyGroupChatsByUserIdAsync(IEnumerable<GroupChatUserModel> myGroupChatUsers, string refreshToken)
    {
        HttpResponseMessage response;
        var container = new List<MyGroupChatContainerModel>();

        foreach (var groupChatUser in myGroupChatUsers)
        {
            response = await _httpClientHelper.GetAsync($"GroupChat/{groupChatUser.ChatId}", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
            if (groupChat == null)
            {
                throw new ArgumentNullException(nameof(groupChat));
            }

            response = await _httpClientHelper.GetAsync($"GroupChatMessageCount/findMe?chatId={groupChatUser.ChatId}&chatUserId={groupChatUser.Id}", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var messageCount = await response.Content.ReadFromJsonAsync<GroupChatMessageCountModel>();
            if (messageCount == null)
            {
                throw new ArgumentNullException(nameof(messageCount));
            }

            container.Add(new MyGroupChatContainerModel { GroupChat = groupChat, GroupChatMessageCount = messageCount });

            if (_groupChatHubConnection == null)
            {
                throw new ArgumentNullException(nameof(_groupChatHubConnection));
            }

            await _groupChatHubConnection.JoinUnreadMessageRoomAsync(groupChatUser.ChatId);
            _groupChatHubConnection.SubscribeUnreadMessagesUpdated(groupChatUser.Id, async (chatId, meInChatId, count) =>
            {
                await UpdateGroupChatUnreadMessagesAsync(chatId, meInChatId, count);
            });
        }

        MyGroupChats = new ObservableCollection<MyGroupChatContainerModel>(container);
    }

    private async Task CreatePersonalChatContainerAsync(IEnumerable<PersonalChatModel> myPersonalChats, string refreshToken)
    {
        HttpResponseMessage response;
        var container = new List<MyPersonalChatContainerModel>();

        foreach (var chat in myPersonalChats)
        {
            await GetPersonalChatCompanionAsync(chat, refreshToken);

            response = await _httpClientHelper.GetAsync($"PersonalChatMessageCount/findMe?chatId={chat.Id}&appUserId={MyAccount?.Id}", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var messageCount = await response.Content.ReadFromJsonAsync<PersonalChatMessageCountModel>();
            if (messageCount == null)
            {
                throw new ArgumentNullException(nameof(messageCount));
            }

            container.Add(new MyPersonalChatContainerModel { PersonalChat = chat, PersonalChatMessageCount = messageCount });

            if (_personalChatHubConnection == null)
            {
                throw new ArgumentNullException(nameof(_personalChatHubConnection));
            }
            else if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }

            await _personalChatHubConnection.JoinUnreadMessageRoomAsync(chat.Id);
            _personalChatHubConnection.SubscribeUnreadMessagesUpdated(MyAccount.Id, async (chatId, meInChatId, count) =>
            {
                await UpdatePersonalChatUnreadMessagesAsync(chatId, meInChatId, count);
            });
        }

        MyPersonalChats = new ObservableCollection<MyPersonalChatContainerModel>(container);
    }

    private async Task UpdatePersonalChatUnreadMessagesAsync(int chatId, string meInChatId, int count)
    {
        try
        {
            if (MyPersonalChats == null)
            {
                throw new ArgumentNullException(nameof(MyPersonalChats));
            }

            var chat = MyPersonalChats.FirstOrDefault(x => x.PersonalChat.Id == chatId);
            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat));
            }
            
            if (chat.PersonalChatMessageCount.AppUserId != meInChatId)
            {
                return;
            }

            var index = MyPersonalChats.IndexOf(chat);
            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                chat.PersonalChatMessageCount.Count = count;
                MyPersonalChats[index] = new MyPersonalChatContainerModel { PersonalChat = chat.PersonalChat, PersonalChatMessageCount = chat.PersonalChatMessageCount };
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

    private async Task UpdateGroupChatUnreadMessagesAsync(int chatId, string meInChatId, int count)
    {
        try
        {
            if (MyGroupChats == null)
            {
                throw new ArgumentNullException(nameof(MyGroupChats));
            }

            var chat = MyGroupChats.FirstOrDefault(x => x.GroupChat.Id == chatId);
            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat));
            }

            if (chat.GroupChatMessageCount.GroupChatUserId != meInChatId)
            {
                return;
            }

            var index = MyGroupChats.IndexOf(chat);
            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                chat.GroupChatMessageCount.Count = count;
                MyGroupChats[index] = new MyGroupChatContainerModel { GroupChat = chat.GroupChat, GroupChatMessageCount = chat.GroupChatMessageCount };
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

    private async Task GetPersonalChatCompanionAsync(PersonalChatModel personalChat, string refreshToken)
    {
        var companionId = personalChat.CompanionId == MyAccount?.Id ? personalChat.InitiatorId : personalChat.CompanionId;
        var response = await _httpClientHelper.GetAsync($"Account/{companionId}", refreshToken, Port.UserApi);
        response.EnsureSuccessStatusCode();

        var companion = await response.Content.ReadFromJsonAsync<AppUserModel>();
        if (companion == null)
        {
            throw new ArgumentNullException(nameof(companion));
        }

        personalChat.Username = companion?.Username ?? string.Empty;
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            var response = await _httpClientHelper.GetAsync("Account", refreshToken, Port.UserApi);
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<AppUserModel>>();
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            _allUsers = users;

            var freeUsers = ExcludeUsersThatAlreadyHasChat();

            Users = new ObservableCollection<AppUserModel>(freeUsers);
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

    private List<AppUserModel> ExcludeUsersThatAlreadyHasChat()
    {
        if (MyPersonalChats == null)
        {
            return new List<AppUserModel>();
        }

        var freeUsers = _allUsers?
            .Where(user => !MyPersonalChats.Any(chat => chat.PersonalChat.InitiatorId == user.Id || chat.PersonalChat.CompanionId == user.Id))
            .Where(user => user.Id != MyAccount?.Id)
            .ToList();

        return freeUsers ?? new List<AppUserModel>();
    }

    private void LoadAppUserUsernameByStartChars(string username)
    {
        if (_allUsers == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(username))
        {
            var usernameByStartChars = _allUsers.Where(x => x.Username.StartsWith(username));
            if (usernameByStartChars == null)
            {
                return;
            }

            Users = new ObservableCollection<AppUserModel>(usernameByStartChars.ToList());
        }
        else
        {
            Users = new ObservableCollection<AppUserModel>(_allUsers);
        }
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
    }

    private async Task InitChatSignalRAsync()
    {
        try
        {
            if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }

            await _personalChatHubConnection.ConnectToUnreadMessageHubAsync($"{Hubs.Port}{Hubs.PersonalChatUnreadMessageAddress}");
            await _groupChatHubConnection.ConnectToUnreadMessageHubAsync($"{Hubs.Port}{Hubs.GroupChatUnreadMessageAddress}");
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
}
