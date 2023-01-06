using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
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

    private IImprovedMvxViewModel _personalChatMessagesTemplate;
    private IImprovedMvxViewModel _groupChatMessagesTemplate;
    private IMemoryCache _memoryCache;
    private ObservableCollection<GroupChatModel> _myGroupChats;
    private ObservableCollection<GroupChatModel> _groupChats;
    private ObservableCollection<PersonalChatModel> _personalChats;
    private ObservableCollection<AppUserModel> _users;
    private List<AppUserModel> _allUsers;
    private List<GroupChatModel> _allGroupChats;
    private string _inputedUserEmail;
    private string _inputedGroupChatName;
    private int _selectedUsersIndex = -1;
    private int _selectedGroupChatsIndex = -1;
    private GroupChatModel _selectedMyGroupChat;
    private PersonalChatModel _selectedPersonalChat;
    private int _selectedMyGroupChatIndex = -1;
    private int _selectedPersonalChatIndex = -1;
    private LoadingStatus _groupChatLoadingResponse;
    private LoadingStatus _personalChatLoadingResponse;
    private AppUserModel _myAccount;

    public ChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        RefreshGroupChatsCommand = new MvxAsyncCommand(LoadGroupChatsAsync);
        RefreshPersonalChatsCommand = new MvxAsyncCommand(LoadPersonalChatsAsync);
        CreatePersonalChatCommand = new MvxAsyncCommand(CreatePersonalChatAsync);
        JoinToGroupChatCommand = new MvxAsyncCommand(JoinToGroupChatAsync);

        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), -2);

        PersonalChatMessagesTemplate = Mvx.IoCProvider.IoCConstruct<PersonalChatMessagesVewModel>();
        GroupChatMessagesTemplate = Mvx.IoCProvider.IoCConstruct<GroupChatMessagesViewModel>();

        GetMyAccount();
    }

    #region Commands

    public IMvxAsyncCommand RefreshGroupChatsCommand { get; set; }

    public IMvxAsyncCommand CreatePersonalChatCommand { get; set; }

    public IMvxAsyncCommand RefreshPersonalChatsCommand { get; set; }

    public IMvxAsyncCommand JoinToGroupChatCommand { get; set; }

    #endregion

    #region Properties

    public IImprovedMvxViewModel PersonalChatMessagesTemplate
    {
        get { return _personalChatMessagesTemplate; }

        set
        {
            SetProperty(ref _personalChatMessagesTemplate, value);
        }
    }

    public IImprovedMvxViewModel GroupChatMessagesTemplate
    {
        get { return _groupChatMessagesTemplate; }

        set
        {
            SetProperty(ref _groupChatMessagesTemplate, value);
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
                SelectedPersonalChatIndex = -1;

                GroupChatMessagesTemplate.Handler.PropertyUpdate<GroupChatMessagesViewModel>(GroupChatMessagesTemplate, nameof(GroupChatMessagesViewModel.SelectedChat), value);
                GroupChatMessagesTemplate.Handler.PropertyUpdate<GroupChatMessagesViewModel>(GroupChatMessagesTemplate, nameof(GroupChatMessagesViewModel.ChatId), MyGroupChats[SelectedMyGroupChatIndex].Id);
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
                SelectedMyGroupChatIndex = -1;
                PersonalChatMessagesTemplate.Handler.PropertyUpdate<PersonalChatMessagesVewModel>(PersonalChatMessagesTemplate, nameof(PersonalChatMessagesVewModel.SelectedChat), value);
            }
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
                        && PersonalChats?.Count == 0;
        }
    }

    public bool IsLoadPersonalChatList
    {
        get
        {
            return (int)PersonalChatLoadingResponse > 0 && (int)PersonalChatLoadingResponse < 3
                        && PersonalChats?.Count > 0;
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

    private async Task LoadUsersAsync()
    {
        _httpClientHelper.BaseAddress = Port.UserApi;

        var response = await _httpClientHelper.GetAsync("Account").ConfigureAwait(true);
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
