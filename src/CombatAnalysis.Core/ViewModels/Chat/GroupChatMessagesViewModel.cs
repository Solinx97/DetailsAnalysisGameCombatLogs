using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class GroupChatMessagesViewModel : MvxViewModel, IImprovedMvxViewModel
{
    private const int MessagesUpdateTimeIsMs = 500;

    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMemoryCache _memoryCache;

    private Timer _messagesUpdateTimer;
    private ObservableCollection<GroupChatMessageModel> _messages;
    private IEnumerable<GroupChatMessageModel> _allMessages;
    private ObservableCollection<AppUserModel> _usersToInviteToChat;
    private ObservableCollection<AppUserModel> _users;
    private List<AppUserModel> _usersExcludingInvitees;
    private GroupChatModel _selectedChat;
    private GroupChatMessageModel _selectedMessage;
    private int _selectedMessageIndex = -1;
    private string _selectedChatName;
    private string _message;
    private bool _chatMenuIsVisibly;
    private AppUserModel _myAccount;
    private LoadingStatus _addUserToChatResponse;
    private bool _inviteToChatIsVisibly;
    private int _selectedUsersForInviteToGroupChatIndex = -1;
    private string _inputedUserEmailForInviteToChat;
    private bool _isEditMode;
    private bool _isRemoveMode;

    public GroupChatMessagesViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache)
    {
        Handler = new VMHandler();

        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;

        SendMessageCommand = new MvxAsyncCommand(SendMessageAsync);
        ShowChatMenuCommand = new MvxCommand(ShowChatMenu);
        OpenInviteToChatCommand = new MvxAsyncCommand(OpenInviteToChatAsync);
        InviteToChatCommand = new MvxAsyncCommand(InviteToChatAsync);
        CloseInviteToChatCommand = new MvxCommand(SwitchInviteToGroupChat);
        TurnOnEditModeCommand = new MvxCommand(() => IsEditMode = !IsEditMode);
        EditMessageCommand = new MvxAsyncCommand(EditMessageAsync);
        RemoveMessageCommand = new MvxAsyncCommand(RemoveMessageAsync);

        Messages = new ObservableCollection<GroupChatMessageModel>();
        _allMessages = new List<GroupChatMessageModel>();

        GetMyAccount();
        Task.Run(LoadUsersAsync);
    }

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

    #region Properties

    public ObservableCollection<AppUserModel> Users
    {
        get { return _users; }
        set
        {
            SetProperty(ref _users, value);
        }
    }

    public ObservableCollection<GroupChatMessageModel> Messages
    {
        get { return _messages; }
        set
        {
            SetProperty(ref _messages, value);
        }
    }

    public GroupChatModel SelectedChat
    {
        get { return _selectedChat; }
        set
        {
            SetProperty(ref _selectedChat, value);

            if (value != null)
            {
                AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    Messages.Clear();
                });

                SelectedChatName = value.Name;

                Task.Run(LoadMessagesAsync);
                AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    Fill();
                });

                _messagesUpdateTimer = new Timer(InitLoadMessages, null, MessagesUpdateTimeIsMs, Timeout.Infinite);
            }
        }
    }

    public GroupChatMessageModel SelectedMessage
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

    public string SelectedChatName
    {
        get { return _selectedChatName; }
        set
        {
            SetProperty(ref _selectedChatName, value);
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

    public bool ChatMenuIsVisibly
    {
        get { return _chatMenuIsVisibly; }
        set
        {
            SetProperty(ref _chatMenuIsVisibly, value);
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

    public LoadingStatus AddUserToChatResponse
    {
        get { return _addUserToChatResponse; }
        set
        {
            SetProperty(ref _addUserToChatResponse, value);
        }
    }

    public ObservableCollection<AppUserModel> UsersToInviteToChat
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

    public string InputedUserEmailForInviteToChat
    {
        get { return _inputedUserEmailForInviteToChat; }
        set
        {
            SetProperty(ref _inputedUserEmailForInviteToChat, value);
            LoadUsersEmailForInviteByStartChars(value);
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

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    #endregion

    public void Fill()
    {
        foreach (var item in _allMessages)
        {
            if (item.GroupChatId == SelectedChat?.Id
                && !Messages.Any(x => x.Id == item.Id))
            {
                Messages.Add(item);
            }
        }
    }

    public void ShowChatMenu()
    {
        ChatMenuIsVisibly = !ChatMenuIsVisibly;
    }

    public async Task SendMessageAsync()
    {
        var newGroupChatMessage = new GroupChatMessageModel
        {
            Message = Message,
            Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}"),
            Username = MyAccount.Email,
            GroupChatId = SelectedChat.Id,
        };

        Message = string.Empty;

        _httpClientHelper.BaseAddress = Port.ChatApi;
        await _httpClientHelper.PostAsync("GroupChatMessage", JsonContent.Create(newGroupChatMessage));
        await _httpClientHelper.PutAsync("GroupChat", JsonContent.Create(SelectedChat));
    }

    public async Task OpenInviteToChatAsync()
    {
        AddUserToChatResponse = LoadingStatus.None;
        UsersToInviteToChat?.Clear();

        var response = await _httpClientHelper.GetAsync("GroupChatUser");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return;
        }

        var groupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
        _usersExcludingInvitees = Users.Where(x => !groupChatUsers.Any(y => x.Id == y.CustomerId)).ToList();

        UsersToInviteToChat = new ObservableCollection<AppUserModel>(_usersExcludingInvitees);

        SwitchInviteToGroupChat();
    }

    public void SwitchInviteToGroupChat()
    {
        InviteToChatIsVisibly = !InviteToChatIsVisibly;
    }

    public async Task InviteToChatAsync()
    {
        var userId = UsersToInviteToChat[SelectedUsersForInviteToGroupChatIndex].Id;

        var groupChatUser = new GroupChatUserModel
        {
            GroupChatId = SelectedChat.Id,
            CustomerId = userId,
        };

        InputedUserEmailForInviteToChat = string.Empty;

        try
        {
            AddUserToChatResponse = LoadingStatus.Pending;

            var response = await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                SwitchInviteToGroupChat();
                //await OpenInviteToChatAsync();

                AddUserToChatResponse = LoadingStatus.Successful;
            }
            else
            {
                AddUserToChatResponse = LoadingStatus.Failed;
            }
        }
        catch (HttpRequestException ex)
        {
            //_logger.LogError(ex, ex.Message);

            AddUserToChatResponse = LoadingStatus.Failed;
        }
    }

    public async Task EditMessageAsync()
    {
        _httpClientHelper.BaseAddress = Port.ChatApi;
        await _httpClientHelper.PutAsync("GroupChatMessage", JsonContent.Create(SelectedMessage));

        IsEditMode= false;
    }

    public async Task RemoveMessageAsync()
    {
        _httpClientHelper.BaseAddress = Port.ChatApi;
        var response = await _httpClientHelper.DeletAsync($"GroupChatMessage/{SelectedMessage.Id}");
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                Messages.Remove(Messages[SelectedMessageIndex]);
                SelectedMessage = null;
            });
        }
    }

    private void InitLoadMessages(object obj)
    {
        Task.Run(LoadMessagesAsync).Wait();
        AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            Fill();
        });

        _messagesUpdateTimer.Change(MessagesUpdateTimeIsMs, Timeout.Infinite);
    }

    private async Task LoadMessagesAsync()
    {
        _httpClientHelper.BaseAddress = Port.ChatApi;

        var response = await _httpClientHelper.GetAsync($"GroupChatMessage/findByChatId/{SelectedChat?.Id}");
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return;
        }

        _allMessages = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();
    }

    private async Task LoadUsersAsync()
    {
        _httpClientHelper.BaseAddress = Port.UserApi;

        var response = await _httpClientHelper.GetAsync("Account");
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();

        //_allUsers = users.Where(x => x.Id != MyAccount?.Id).ToList();

        Users = new ObservableCollection<AppUserModel>(users.Where(x => x.Id != MyAccount?.Id).ToList());
    }

    private void LoadUsersEmailForInviteByStartChars(string startChars)
    {
        if (!string.IsNullOrEmpty(startChars))
        {
            var usersEmailByStartChars = Users.Where(x => x.Email.StartsWith(startChars));

            UsersToInviteToChat = new ObservableCollection<AppUserModel>(usersEmailByStartChars);
        }
        else
        {
            UsersToInviteToChat = new ObservableCollection<AppUserModel>(_usersExcludingInvitees);
        }
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
    }
}
