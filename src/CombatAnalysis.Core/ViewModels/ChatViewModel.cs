using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels;

public class ChatViewModel : MvxViewModel, IImprovedMvxViewModel
{
    private const int GroupChatMessagesUpdateTimeIsMs = 500;
    private const int PersonalChatMessagesUpdateTimeIsMs = 500;

    private readonly IHttpClientHelper _httpClientHelper;
    private readonly ILogger _logger;

    private IImprovedMvxViewModel _basicTemplate;
    private IMemoryCache _memoryCache;
    private ObservableCollection<GroupChatMessageModel> _groupChatMessages;
    private ObservableCollection<PersonalChatMessageModel> _personalChatMessages;
    private ObservableCollection<GroupChatModel> _myGroupChats;
    private ObservableCollection<GroupChatModel> _groupChats;
    private ObservableCollection<PersonalChatModel> _personalChats;
    private ObservableCollection<AppUserModel> _users;
    private ObservableCollection<AppUserModel> _usersForInviteToGroupChat;
    private List<AppUserModel> _allUsers;
    private List<GroupChatModel> _allGroupChats;
    private string _inputedUserEmail;
    private string _inputedUserEmailForInviteToGroupChat;
    private string _inputedGroupChatName;
    private int _selectedUsersIndex = -1;
    private int _selectedUsersForInviteToGroupChatIndex = -1;
    private int _selectedGroupChatsIndex = -1;
    private bool _isMyMessage = true;
    private string _selectedChatName;
    private GroupChatModel _selectedMyGroupChat;
    private PersonalChatModel _selectedPersonalChat;
    private int _selectedMyGroupChatIndex = -1;
    private int _selectedPersonalChatIndex = -1;
    private string _message;
    private bool _groupChatMenuIsVisibly;
    private LoadingStatus _groupChatLoadingResponse;
    private LoadingStatus _personalChatLoadingResponse;
    private LoadingStatus _addUserToGroupChatResponse;
    private Timer _groupChatMessagesUpdateTimer;
    private Timer _personalChatMessagesUpdateTimer;
    private AppUserModel _myAccount;
    private bool _inviteToGroupChatIsVisibly;

    public ChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        GroupChatMessages = new ObservableCollection<GroupChatMessageModel>();
        SendGroupMessageCommand = new MvxAsyncCommand(SendGroupMessageAsync);
        SendPersonalMessageCommand = new MvxAsyncCommand(SendPersonalMessageAsync);
        RefreshGroupChatsCommand = new MvxAsyncCommand(LoadGroupChatsAsync);
        RefreshPersonalChatsCommand = new MvxAsyncCommand(LoadPersonalChatsAsync);
        ShowGroupChatMenuCommand = new MvxCommand(ShowGCMenu);
        CreatePersonalChatCommand = new MvxAsyncCommand(CreatePersonalChatAsync);
        OpenInviteToGroupChatCommand = new MvxAsyncCommand(OpenInviteToGroupChatAsync);
        CloseInviteToGroupChatCommand = new MvxCommand(SwitchInviteToGroupChat);
        InviteToGroupChatCommand = new MvxAsyncCommand(InviteToGroupChatAsync);
        JoinToGroupChatCommand = new MvxAsyncCommand(JoinToGroupChatAsync);

        BasicTemplate = Templates.Basic;
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), -2);

        GetMyAccount();
    }

    #region Commands

    public IMvxAsyncCommand SendGroupMessageCommand { get; set; }

    public IMvxAsyncCommand SendPersonalMessageCommand { get; set; }

    public IMvxAsyncCommand RefreshGroupChatsCommand { get; set; }

    public IMvxAsyncCommand CreatePersonalChatCommand { get; set; }

    public IMvxAsyncCommand RefreshPersonalChatsCommand { get; set; }

    public IMvxCommand ShowGroupChatMenuCommand { get; set; }

    public IMvxAsyncCommand OpenInviteToGroupChatCommand { get; set; }

    public IMvxCommand CloseInviteToGroupChatCommand { get; set; }

    public IMvxAsyncCommand InviteToGroupChatCommand { get; set; }

    public IMvxAsyncCommand JoinToGroupChatCommand { get; set; }

    #endregion

    #region Properties

    public IViewModelConnect Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    public IImprovedMvxViewModel BasicTemplate
    {
        get { return _basicTemplate; }

        set
        {
            SetProperty(ref _basicTemplate, value);
        }
    }

    public ObservableCollection<GroupChatMessageModel> GroupChatMessages
    {
        get { return _groupChatMessages; }
        set
        {
            SetProperty(ref _groupChatMessages, value);
        }
    }

    public ObservableCollection<PersonalChatMessageModel> PersonalChatMessages
    {
        get { return _personalChatMessages; }
        set
        {
            SetProperty(ref _personalChatMessages, value);
        }
    }

    public ObservableCollection<AppUserModel> Users
    {
        get { return _users; }
        set
        {
            SetProperty(ref _users, value);
        }
    }

    public ObservableCollection<AppUserModel> UsersForInviteToGroupChat
    {
        get { return _usersForInviteToGroupChat; }
        set
        {
            SetProperty(ref _usersForInviteToGroupChat, value);
        }
    }

    public ObservableCollection<GroupChatModel> GroupChats
    {
        get { return _groupChats; }
        set
        {
            SetProperty(ref _groupChats, value);
        }
    }

    public ObservableCollection<GroupChatModel> MyGroupChats
    {
        get { return _myGroupChats; }
        set
        {
            SetProperty(ref _myGroupChats, value);
        }
    }

    public ObservableCollection<PersonalChatModel> PersonalChats
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

    public int SelectedUsersForInviteToGroupChatIndex
    {
        get { return _selectedUsersForInviteToGroupChatIndex; }
        set
        {
            SetProperty(ref _selectedUsersForInviteToGroupChatIndex, value);
        }
    }

    public int SelectedGroupChatsIndex
    {
        get { return _selectedGroupChatsIndex; }
        set
        {
            SetProperty(ref _selectedGroupChatsIndex, value);
        }
    }

    public string InputedUserEmail
    {
        get { return _inputedUserEmail; }
        set
        {
            SetProperty(ref _inputedUserEmail, value);
            LoadUsersEmailByStartChars(value);
        }
    }

    public string InputedUserEmailForInviteToGroupChat
    {
        get { return _inputedUserEmailForInviteToGroupChat; }
        set
        {
            SetProperty(ref _inputedUserEmailForInviteToGroupChat, value);
            LoadUsersEmailForInviteByStartChars(value);
        }
    }

    public string InputedGroupChatName
    {
        get { return _inputedGroupChatName; }
        set
        {
            SetProperty(ref _inputedGroupChatName, value);
            LoadGroupChatsNameByStartChars(value);
        }
    }

    public int SelectedMyGroupChatIndex
    {
        get { return _selectedMyGroupChatIndex; }
        set
        {
            SetProperty(ref _selectedMyGroupChatIndex, value);
        }
    }

    public GroupChatModel SelectedMyGroupChat
    {
        get { return _selectedMyGroupChat; }
        set
        {
            SetProperty(ref _selectedMyGroupChat, value);

            if (value != null)
            {
                SelectedChatName = value.Name;
                _groupChatMessagesUpdateTimer = new Timer(InitLoadGroupChatMessages, null, GroupChatMessagesUpdateTimeIsMs, Timeout.Infinite);
            }
        }
    }

    public int SelectedPersonalChatIndex
    {
        get { return _selectedPersonalChatIndex; }
        set
        {
            SetProperty(ref _selectedPersonalChatIndex, value);
        }
    }

    public PersonalChatModel SelectedPersonalChat
    {
        get { return _selectedPersonalChat; }
        set
        {
            SetProperty(ref _selectedPersonalChat, value);

            if (value != null)
            {
                SelectedChatName = MyAccount.Id == value.InitiatorId ? value.CompanionUsername : value.InitiatorUsername;
                _personalChatMessagesUpdateTimer = new Timer(InitLoadPersonalChatMessages, null, PersonalChatMessagesUpdateTimeIsMs, Timeout.Infinite);
            }
        }
    }

    public string SelectedChatName
    {
        get { return _selectedChatName; }
        set
        {
            SetProperty(ref _selectedChatName, value);
        }
    }

    public bool IsMyMessage
    {
        get { return _isMyMessage; }
        set
        {
            SetProperty(ref _isMyMessage, value);
        }
    }

    public string Message
    {
        get { return _message; }
        set
        {
            SetProperty(ref _message, value);
        }
    }

    public AppUserModel MyAccount
    {
        get { return _myAccount; }
        set
        {
            SetProperty(ref _myAccount, value);
        }
    }
    
    public bool GroupChatMenuIsVisibly
    {
        get { return _groupChatMenuIsVisibly; }
        set
        {
            SetProperty(ref _groupChatMenuIsVisibly, value);
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

    public LoadingStatus AddUserToGroupChatResponse
    {
        get { return _addUserToGroupChatResponse; }
        set
        {
            SetProperty(ref _addUserToGroupChatResponse, value);
        }
    }

    public bool InviteToGroupChatIsVisibly
    {
        get { return _inviteToGroupChatIsVisibly; }
        set
        {
            SetProperty(ref _inviteToGroupChatIsVisibly, value);
        }
    }

    public bool IsShowEmptyMyGroupChat
    {
        get
        {
            return (int)GroupChatLoadingResponse > 0 && (int)GroupChatLoadingResponse < 3
                        && (MyGroupChats?.Count == 0);
        }
    }

    public bool IsLoadMyGroupChatList
    {
        get 
        { 
            return (int)GroupChatLoadingResponse > 0 && (int)GroupChatLoadingResponse < 3
                        && (MyGroupChats?.Count > 0);
        }
    }

    public bool IsShowEmptyPersonalChat
    {
        get
        {
            return (int)PersonalChatLoadingResponse > 0 && (int)PersonalChatLoadingResponse < 3
                        && (PersonalChats?.Count == 0);
        }
    }

    public bool IsLoadPersonalChatList
    {
        get
        {
            return (int)PersonalChatLoadingResponse > 0 && (int)PersonalChatLoadingResponse < 3
                        && (PersonalChats?.Count > 0);
        }
    }

    #endregion

    public override void Prepare()
    {
        base.Prepare();

        Task.Run(LoadGroupChatsAsync);
        Task.Run(LoadPersonalChatsAsync);
        Task.Run(LoadUsersAsync);
    }

    public async Task SendGroupMessageAsync()
    {
        var newGroupChatMessage = new GroupChatMessageModel
        {
            Message = Message,
            Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}"),
            Username = MyAccount.Email,
            GroupChatId = SelectedMyGroupChat.Id,
        };

        Message = string.Empty;

        _httpClientHelper.BaseAddress = Port.ChatApi;
        await _httpClientHelper.PostAsync("GroupChatMessage", JsonContent.Create(newGroupChatMessage));

        SelectedMyGroupChat.LastMessage = newGroupChatMessage.Message;

        await _httpClientHelper.PutAsync("GroupChat", JsonContent.Create(SelectedMyGroupChat));
    }

    public async Task SendPersonalMessageAsync()
    {
        var newPersonalChatMessage = new PersonalChatMessageModel
        {
            Message = Message,
            Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}"),
            Username = MyAccount.Email,
            PersonalChatId = SelectedPersonalChat.Id,
        };

        Message = string.Empty;

        _httpClientHelper.BaseAddress = Port.ChatApi;
        await _httpClientHelper.PostAsync("PersonalChatMessage", JsonContent.Create(newPersonalChatMessage));

        SelectedPersonalChat.LastMessage = newPersonalChatMessage.Message;

        await _httpClientHelper.PutAsync("PersonalChat", JsonContent.Create(SelectedPersonalChat));
    }

    public void ShowGCMenu()
    {
        GroupChatMenuIsVisibly = !GroupChatMenuIsVisibly;
    }

    public async Task CreatePersonalChatAsync()
    {
        var userId = Users[SelectedUsersIndex].Id;
        var personalChat = new PersonalChatModel
        {
            InitiatorId = MyAccount.Id,
            InitiatorUsername = MyAccount.Email,
            CompanionId = userId,
            CompanionUsername = Users[SelectedUsersIndex].Email,
            LastMessage = " ",
        };

        InputedUserEmail = string.Empty;

        _httpClientHelper.BaseAddress = Port.ChatApi;
        var response = await _httpClientHelper.PostAsync("PersonalChat/personalChatIsAlreadyExists", JsonContent.Create(personalChat));
        var personalChatId = await response.Content.ReadFromJsonAsync<int>();

        if (personalChatId > 0)
        {
            var existPersonChat = PersonalChats?.FirstOrDefault(x => x.Id == personalChatId);
            if (existPersonChat != null)
            {
                SelectedPersonalChatIndex = PersonalChats.IndexOf(existPersonChat);
            }
        }
        else
        {
            await _httpClientHelper.PostAsync("PersonalChat", JsonContent.Create(personalChat));
        }
    }

    public void SwitchInviteToGroupChat()
    {
        InviteToGroupChatIsVisibly = !InviteToGroupChatIsVisibly;
    }

    public async Task InviteToGroupChatAsync()
    {
        var myGroupChatId = MyGroupChats[SelectedMyGroupChatIndex].Id;
        var userId = UsersForInviteToGroupChat[SelectedUsersForInviteToGroupChatIndex].Id;

        var groupChatUser = new GroupChatUserModel
        {
            GroupChatId = myGroupChatId,
            UserId = userId,
        };

        InputedUserEmailForInviteToGroupChat = string.Empty;

        try
        {
            AddUserToGroupChatResponse = LoadingStatus.Pending;

            var response = await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                SwitchInviteToGroupChat();
                await OpenInviteToGroupChatAsync();

                AddUserToGroupChatResponse = LoadingStatus.Successful;
            }
            else
            {
                AddUserToGroupChatResponse = LoadingStatus.Failed;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            AddUserToGroupChatResponse = LoadingStatus.Failed;
        }
    }

    public async Task JoinToGroupChatAsync()
    {
        var groupChatId = GroupChats[SelectedGroupChatsIndex].Id;

        var groupChatUser = new GroupChatUserModel
        {
            GroupChatId = groupChatId,
            UserId = MyAccount.Id,
        };

        InputedGroupChatName = string.Empty;

        await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser));

        await LoadGroupChatsAsync();
    }

    public async Task OpenInviteToGroupChatAsync()
    {
        AddUserToGroupChatResponse = LoadingStatus.None;
        UsersForInviteToGroupChat = null;

        var response = await _httpClientHelper.GetAsync("GroupChatUser");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return;
        }

        var groupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
        var usersForInviteToGroupChat = Users.Where(x => !groupChatUsers.Any(y => x.Id == y.UserId)).ToList();

        UsersForInviteToGroupChat = new ObservableCollection<AppUserModel>(usersForInviteToGroupChat);

        SwitchInviteToGroupChat();
    }

    private void InitLoadGroupChatMessages(object obj)
    {
        Task.Run(LoadGroupChatMessagesAsync);

        if (SelectedMyGroupChat != null)
        {
            _groupChatMessagesUpdateTimer.Change(GroupChatMessagesUpdateTimeIsMs, Timeout.Infinite);
        }
    }

    private void InitLoadPersonalChatMessages(object obj)
    {
        Task.Run(LoadPersonalChatMessagesAsync);

        if (SelectedPersonalChat != null)
        {
            _personalChatMessagesUpdateTimer.Change(PersonalChatMessagesUpdateTimeIsMs, Timeout.Infinite);
        }
    }

    private async Task LoadGroupChatsAsync()
    {
        GroupChatLoadingResponse = LoadingStatus.Pending;

        try
        {
            _httpClientHelper.BaseAddress = Port.ChatApi;
            var response = await _httpClientHelper.GetAsync("GroupChatUser").ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await GetMyGroupChatsAsync(response);
                await GetGroupChatsAsync();
                
                GroupChatLoadingResponse = LoadingStatus.Successful;
            }
            else
            {
                GroupChatLoadingResponse = LoadingStatus.Failed;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
    }

    private async Task GetMyGroupChatsAsync(HttpResponseMessage response)
    {
        var groupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
        var myGroupChatUsers = groupChatUsers.Where(x => x.UserId == MyAccount.Id);
        var myGroupChats = new List<GroupChatModel>();
        foreach (var item in myGroupChatUsers)
        {
            response = await _httpClientHelper.GetAsync($"GroupChat/{item.GroupChatId}");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
                myGroupChats.Add(groupChat);
            }
        }

        MyGroupChats = new ObservableCollection<GroupChatModel>(myGroupChats);
    }

    private async Task GetGroupChatsAsync()
    {
        var response = await _httpClientHelper.GetAsync("GroupChat");
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return;
        }

        var groupChats = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatModel>>();
        var groupChatsExcludeMe = groupChats.Where(x => !MyGroupChats.Any(y => x.Id == y.Id)).ToList();

        _allGroupChats = groupChatsExcludeMe.ToList();
        GroupChats = new ObservableCollection<GroupChatModel>(groupChatsExcludeMe);
    }

    private async Task LoadPersonalChatsAsync()
    {
        PersonalChatLoadingResponse = LoadingStatus.Pending;

        try
        {
            _httpClientHelper.BaseAddress = Port.ChatApi;
            var response = await _httpClientHelper.GetAsync("PersonalChat").ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var personalChats = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
                var myPersonalChats = personalChats.Where(x => x.InitiatorId == MyAccount?.Id || x.CompanionId == MyAccount?.Id).ToList();

                PersonalChats = new ObservableCollection<PersonalChatModel>(myPersonalChats);

                PersonalChatLoadingResponse = LoadingStatus.Successful;
            }
            else
            {
                PersonalChatLoadingResponse = LoadingStatus.Failed;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
    }

    private async Task LoadGroupChatMessagesAsync()
    {
        _httpClientHelper.BaseAddress = Port.ChatApi;

        var response = await _httpClientHelper.GetAsync("GroupChatMessage").ConfigureAwait(false);
        var groupChatMessages = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();
        var selectedGroupChatMessages = new List<GroupChatMessageModel>();
        foreach (var item in groupChatMessages)
        {
            if (item.GroupChatId == SelectedMyGroupChat?.Id)
            {
                selectedGroupChatMessages.Add(item);
            }
        }

        GroupChatMessages = new ObservableCollection<GroupChatMessageModel>(selectedGroupChatMessages);
    }

    private async Task LoadPersonalChatMessagesAsync()
    {
        _httpClientHelper.BaseAddress = Port.ChatApi;

        var response = await _httpClientHelper.GetAsync("PersonalChatMessage");
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return;
        }

        var personalChatMessages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();
        var selectedPersonalChatMessages = new List<PersonalChatMessageModel>();
        foreach (var item in personalChatMessages)
        {
            if (item.PersonalChatId == SelectedPersonalChat?.Id)
            {
                selectedPersonalChatMessages.Add(item);
            }
        }

        PersonalChatMessages = new ObservableCollection<PersonalChatMessageModel>(selectedPersonalChatMessages);
    }

    private async Task LoadUsersAsync()
    {
        _httpClientHelper.BaseAddress = Port.UserApi;

        var response = await _httpClientHelper.GetAsync("Account").ConfigureAwait(false);
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();

        _allUsers = users.ToList();
        foreach (var item in _allUsers)
        {
            if (item.Id == MyAccount?.Id)
            {
                _allUsers.Remove(item);
                break;
            }
        }

        Users = new ObservableCollection<AppUserModel>(_allUsers);
    }

    private void LoadUsersEmailByStartChars(string startChars)
    {
        if (!string.IsNullOrEmpty(startChars))
        {
            var usersEmailByStartChars = _allUsers.Where(x => x.Email.StartsWith(startChars));

            Users = new ObservableCollection<AppUserModel>(usersEmailByStartChars);
        }
        else
        {
            Users = new ObservableCollection<AppUserModel>(_allUsers);
        }
    }

    private void LoadUsersEmailForInviteByStartChars(string startChars)
    {
        if (!string.IsNullOrEmpty(startChars))
        {
            var usersEmailByStartChars = _allUsers.Where(x => x.Email.StartsWith(startChars));

            UsersForInviteToGroupChat = new ObservableCollection<AppUserModel>(usersEmailByStartChars);
        }
        else
        {
            UsersForInviteToGroupChat = new ObservableCollection<AppUserModel>(_allUsers);
        }
    }

    private void LoadGroupChatsNameByStartChars(string startChars)
    {
        if (!string.IsNullOrEmpty(startChars))
        {
            var groupChatsNameByStartChars = _allGroupChats.Where(x => x.Name.StartsWith(startChars));

            GroupChats = new ObservableCollection<GroupChatModel>(groupChatsNameByStartChars);
        }
        else
        {
            GroupChats = new ObservableCollection<GroupChatModel>(_allGroupChats);
        }
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>("account");
    }
}
