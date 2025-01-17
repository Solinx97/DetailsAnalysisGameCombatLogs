using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.Containers;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using System.Collections.ObjectModel;
using System.Net;
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
    private HubConnection? _personalChatHhubConnection;
    private HubConnection? _grouplChatHhubConnection;

    public ChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        RefreshGroupChatsCommand = new MvxAsyncCommand(LoadGroupChatsAsync);
        RefreshPersonalChatsCommand = new MvxAsyncCommand(LoadPersonalChatsAsync);
        CreatePersonalChatCommand = new MvxAsyncCommand(CreatePersonalChatAsync);

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
                LoadCustomersUsernameByStartChars(value);
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

    private async Task CreatePersonalChatAsync()
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

            var targetCustomer = Users[SelectedUsersIndex];
            var personalChat = new PersonalChatModel
            {
                InitiatorId = MyAccount.Id,
                CompanionId = targetCustomer.Id,
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

            var myGroupChatUsers = await response.Content.ReadFromJsonAsync<List<GroupChatUserModel>>();
            if (myGroupChatUsers == null)
            {
                throw new ArgumentNullException(nameof(myGroupChatUsers));
            }

            await GetMyGroupChatsAsync(myGroupChatUsers, refreshToken);

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

    private async Task GetMyGroupChatsAsync(IEnumerable<GroupChatUserModel> myGroupChatUsers, string refreshToken)
    {
        try
        {
            HttpResponseMessage response;
            var container = new List<MyGroupChatContainerModel>();

            foreach (var item in myGroupChatUsers)
            {
                response = await _httpClientHelper.GetAsync($"GroupChat/{item.ChatId}", refreshToken, Port.ChatApi);
                response.EnsureSuccessStatusCode();

                var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
                if (groupChat == null)
                {
                    throw new ArgumentNullException(nameof(groupChat));
                }

                response = await _httpClientHelper.GetAsync($"GroupChatMessageCount/findMe?chatId={item.ChatId}&chatUserId={item.Id}", refreshToken, Port.ChatApi);
                response.EnsureSuccessStatusCode();

                var messageCount = await response.Content.ReadFromJsonAsync<GroupChatMessageCountModel>();
                if (messageCount == null)
                {
                    throw new ArgumentNullException(nameof(messageCount));
                }

                container.Add(new MyGroupChatContainerModel { GroupChat = groupChat, GroupChatMessageCount = messageCount });
                
                if (_grouplChatHhubConnection == null)
                {
                    throw new ArgumentNullException(nameof(_grouplChatHhubConnection));
                }

                await _grouplChatHhubConnection.SendAsync("JoinRoom", item.ChatId);
            }

            MyGroupChats = new ObservableCollection<MyGroupChatContainerModel>(container);
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

    private async Task CreatePersonalChatContainerAsync(IEnumerable<PersonalChatModel> myPersonalChats, string refreshToken)
    {
        HttpResponseMessage response;
        var container = new List<MyPersonalChatContainerModel>();

        foreach (var item in myPersonalChats)
        {
            await GetPersonalChatCompanionAsync(item);

            response = await _httpClientHelper.GetAsync($"PersonalChatMessageCount/findMe?chatId={item.Id}&appUserId={MyAccount?.Id}", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var messageCount = await response.Content.ReadFromJsonAsync<PersonalChatMessageCountModel>();
            if (messageCount == null)
            {
                throw new ArgumentNullException(nameof(messageCount));
            }

            container.Add(new MyPersonalChatContainerModel { PersonalChat = item, PersonalChatMessageCount = messageCount });

            if (_personalChatHhubConnection == null)
            {
                throw new ArgumentNullException(nameof(_personalChatHhubConnection));
            }

            await _personalChatHhubConnection.SendAsync("JoinRoom", item.Id);
        }

        MyPersonalChats = new ObservableCollection<MyPersonalChatContainerModel>(container);
    }

    private async Task GetPersonalChatCompanionAsync(PersonalChatModel personalChat)
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

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

    private void LoadCustomersUsernameByStartChars(string username)
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
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            var accessToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.AccessToken));

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }
            else if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri($"{Hubs.Port}{Hubs.PersonalChatUnreadMessageAddress}"), new Cookie(nameof(MemoryCacheValue.RefreshToken), refreshToken));
            cookieContainer.Add(new Uri($"{Hubs.Port}{Hubs.PersonalChatUnreadMessageAddress}"), new Cookie(nameof(MemoryCacheValue.AccessToken), accessToken));

            _personalChatHhubConnection = new HubConnectionBuilder()
                .WithUrl($"{Hubs.Port}{Hubs.PersonalChatUnreadMessageAddress}", options =>
                {
                    options.Cookies = cookieContainer;
                })
                .Build();

            await _personalChatHhubConnection.StartAsync();

            _personalChatHhubConnection.On<int>("ReceiveUnreadMessageIncreased", async (chatId) => {
                await _personalChatHhubConnection.SendAsync("RequestUnreadMessages", chatId, MyAccount?.Id);
            });

            _personalChatHhubConnection.On<int, int>("ReceiveUnreadMessageCount", async (chatId, count) => {
                var chat = MyPersonalChats?.FirstOrDefault(x => x.PersonalChat.Id == chatId) ?? throw new ArgumentNullException(nameof(MyPersonalChats));
                var index = MyPersonalChats.IndexOf(chat);
                await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    chat.PersonalChatMessageCount.Count = count;
                    MyPersonalChats[index] = new MyPersonalChatContainerModel { PersonalChat = chat.PersonalChat, PersonalChatMessageCount = chat.PersonalChatMessageCount };
                });
            });

            cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri($"{Hubs.Port}{Hubs.GroupChatUnreadMessageAddress}"), new Cookie(nameof(MemoryCacheValue.RefreshToken), refreshToken));
            cookieContainer.Add(new Uri($"{Hubs.Port}{Hubs.GroupChatUnreadMessageAddress}"), new Cookie(nameof(MemoryCacheValue.AccessToken), accessToken));

            _grouplChatHhubConnection = new HubConnectionBuilder()
                .WithUrl($"{Hubs.Port}{Hubs.GroupChatUnreadMessageAddress}", options =>
                {
                    options.Cookies = cookieContainer;
                })
                .Build();

            await _grouplChatHhubConnection.StartAsync();

            _grouplChatHhubConnection.On<int>("ReceiveUnreadMessageIncreased", async (chatId) => {
                await _grouplChatHhubConnection.SendAsync("RequestUnreadMessages", chatId, MyAccount?.Id);
            });

            _grouplChatHhubConnection.On<int, int>("ReceiveUnreadMessageCount", async (chatId, count) => {
                var chat = MyPersonalChats?.FirstOrDefault(x => x.PersonalChat.Id == chatId) ?? throw new ArgumentNullException(nameof(MyPersonalChats));
                var index = MyPersonalChats.IndexOf(chat);
                await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    chat.PersonalChatMessageCount.Count = count;
                    MyPersonalChats[index] = new MyPersonalChatContainerModel { PersonalChat = chat.PersonalChat, PersonalChatMessageCount = chat.PersonalChatMessageCount };
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
}
