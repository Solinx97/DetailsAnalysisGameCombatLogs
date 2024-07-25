using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
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

    private bool _isChatSelected;
    private IImprovedMvxViewModel _personalChatMessagesTemplate;
    private IImprovedMvxViewModel _groupChatMessagesTemplate;
    private IMemoryCache _memoryCache;
    private ObservableCollection<GroupChatModel> _myGroupChats;
    private ObservableCollection<PersonalChatModel> _personalChats;
    private ObservableCollection<AppUserModel> _users;
    private List<AppUserModel> _allUsers;
    private string _inputedUsername;
    private int _selectedUsersIndex = -1;
    private GroupChatModel _selectedMyGroupChat;
    private PersonalChatModel _selectedPersonalChat;
    private int _selectedMyGroupChatIndex = -1;
    private int _selectedPersonalChatIndex = -1;
    private LoadingStatus _groupChatLoadingResponse;
    private LoadingStatus _personalChatLoadingResponse;
    private AppUserModel _myAccount;
    private CustomerModel _customer;

    public ChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        RefreshGroupChatsCommand = new MvxAsyncCommand(LoadGroupChatsAsync);
        RefreshPersonalChatsCommand = new MvxAsyncCommand(LoadPersonalChatsAsync);
        CreatePersonalChatCommand = new MvxAsyncCommand(CreatePersonalChatAsync);

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

    #endregion

    #region Properties

    public bool IsChatSelected
    {
        get { return _isChatSelected; }

        set
        {
            SetProperty(ref _isChatSelected, value);
        }
    }

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

    public string InputedUsername
    {
        get { return _inputedUsername; }
        set
        {
            SetProperty(ref _inputedUsername, value);
            LoadCustomersUsernameByStartChars(value);
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
                IsChatSelected = true;

                GroupChatMessagesTemplate.Handler.PropertyUpdate<GroupChatMessagesViewModel>(GroupChatMessagesTemplate, nameof(GroupChatMessagesViewModel.SelectedChat), value);
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
                IsChatSelected = true;

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
        Task.Run(LoadCustomersAsync);
    }

    public async Task CreatePersonalChatAsync()
    {
        var targetCustomer = Users[SelectedUsersIndex];
        var personalChat = new PersonalChatModel
        {
            Username = " ",
            LastMessage = " ",
            InitiatorId = MyAccount.Id,
            CompanionId = targetCustomer.Id,
        };

        InputedUsername = string.Empty;

        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var response = await _httpClientHelper.PostAsync("PersonalChat/personalChatIsAlreadyExists", JsonContent.Create(personalChat), refreshToken, Port.ChatApi);
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
            response = await _httpClientHelper.PostAsync("PersonalChat", JsonContent.Create(personalChat), refreshToken, Port.ChatApi);
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
                return;
            }

            var response = await _httpClientHelper.GetAsync($"GroupChatUser/findByUserId/{MyAccount.Id}", refreshToken, Port.ChatApi);
            if (!response.IsSuccessStatusCode)
            {
                GroupChatLoadingResponse = LoadingStatus.Failed;
                return;
            }

            var myGroupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();

            await GetMyGroupChatsAsync(myGroupChatUsers);

            GroupChatLoadingResponse = LoadingStatus.Successful;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
    }

    private async Task GetMyGroupChatsAsync(IEnumerable<GroupChatUserModel> myGroupChatUsers)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var myGroupChats = new List<GroupChatModel>();
        foreach (var item in myGroupChatUsers)
        {
            var response = await _httpClientHelper.GetAsync($"GroupChat/{item.GroupChatId}", refreshToken, Port.ChatApi);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
                myGroupChats.Add(groupChat);
            }
        }

        MyGroupChats = new ObservableCollection<GroupChatModel>(myGroupChats);
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
            if (!response.IsSuccessStatusCode)
            {
                PersonalChatLoadingResponse = LoadingStatus.Failed;
                return;
            }

            var personalChats = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
            var myPersonalChats = personalChats.Where(x => x.InitiatorId == MyAccount?.Id || x.CompanionId == MyAccount?.Id).ToList();
            foreach (var item in myPersonalChats)
            {
                await GetPersonalChatCompanionAsync(item);
            }

            PersonalChats = new ObservableCollection<PersonalChatModel>(myPersonalChats);

            PersonalChatLoadingResponse = LoadingStatus.Successful;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
    }

    private async Task GetPersonalChatCompanionAsync(PersonalChatModel personalChat)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var companionId = personalChat.CompanionId == MyAccount.Id ? personalChat.InitiatorId : personalChat.CompanionId;
        var response = await _httpClientHelper.GetAsync($"Account/{companionId}", refreshToken, Port.UserApi);
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var companion = await response.Content.ReadFromJsonAsync<AppUserModel>();
        personalChat.Username = companion?.Username;
    }

    private async Task LoadCustomersAsync()
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var response = await _httpClientHelper.GetAsync("Account", refreshToken, Port.UserApi);
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var users = await response.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();
        _allUsers = new List<AppUserModel>();

        foreach (var item in users)
        {
            if (item.Id == MyAccount?.Id)
            {
                continue;
            }

            response = await _httpClientHelper.GetAsync($"Account/{item.Id}", refreshToken, Port.UserApi);
            if (string.IsNullOrEmpty(refreshToken))
            {
                break;
            }

            var user = await response.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();
            _allUsers.Add(user.FirstOrDefault());
        }

        Users = new ObservableCollection<AppUserModel>(_allUsers.ToList());
    }

    private void LoadCustomersUsernameByStartChars(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            var customerUsernameByStartChars = _allUsers.Where(x => x.Username.StartsWith(username));
            Users = new ObservableCollection<AppUserModel>(customerUsernameByStartChars.ToList());
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
}
