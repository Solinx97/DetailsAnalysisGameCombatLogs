using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
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

public class PersonalChatMessagesVewModel : MvxViewModel, IImprovedMvxViewModel
{
    private const int MessagesUpdateTimeIsMs = 250;

    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMemoryCache _memoryCache;

    private Timer _messagesUpdateTimer;
    private ObservableCollection<PersonalChatMessageModel> _messages;
    private IEnumerable<PersonalChatMessageModel> _allMessages;
    private PersonalChatModel _selectedChat;
    private string _selectedChatName;
    private string _message;
    private AppUserModel _myAccount;
    private CustomerModel _customer;

    public PersonalChatMessagesVewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache)
    {
        Handler = new VMHandler();

        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;

        SendMessageCommand = new MvxAsyncCommand(SendMessageAsync);

        Messages = new ObservableCollection<PersonalChatMessageModel>();
        _allMessages = new List<PersonalChatMessageModel>();

        GetMyAccount();
    }

    #region Commands

    public IMvxAsyncCommand SendMessageCommand { get; set; }

    #endregion

    #region Properties

    public ObservableCollection<PersonalChatMessageModel> Messages
    {
        get { return _messages; }
        set
        {
            SetProperty(ref _messages, value);
        }
    }

    public PersonalChatModel SelectedChat
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

                AsyncDispatcher.ExecuteOnMainThreadAsync(async () =>
                {
                    await LoadMessagesAsync();
                    Fill();
                });

                _messagesUpdateTimer = new Timer(InitLoadMessages, null, MessagesUpdateTimeIsMs, Timeout.Infinite);
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

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    #endregion

    public void Fill()
    {
        foreach (var item in _allMessages)
        {
            if (item.ChatId == SelectedChat?.Id
                && !Messages.Any(x => x.Id == item.Id))
            {
                Messages.Add(item);
            }
        }
    }

    public async Task SendMessageAsync()
    {
        var newPersonalChatMessage = new PersonalChatMessageModel
        {
            Message = Message,
            Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}").ToString(),
            Status = 0,
            ChatId = SelectedChat.Id,
            AppUserId = MyAccount.Id,
        };

        Message = string.Empty;

        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        await _httpClientHelper.PostAsync("PersonalChatMessage", JsonContent.Create(newPersonalChatMessage), refreshToken, Port.ChatApi);

        SelectedChat.LastMessage = newPersonalChatMessage.Message;

        await _httpClientHelper.PutAsync("PersonalChat", JsonContent.Create(SelectedChat), refreshToken, Port.ChatApi);
    }

    private void InitLoadMessages(object obj)
    {
        AsyncDispatcher.ExecuteOnMainThreadAsync(async () =>
        {
            await LoadMessagesAsync();
            Fill();
        });

        _messagesUpdateTimer.Change(MessagesUpdateTimeIsMs, Timeout.Infinite);
    }

    private async Task LoadMessagesAsync()
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var response = await _httpClientHelper.GetAsync($"PersonalChatMessage/findByChatId/{SelectedChat?.Id}", refreshToken, Port.ChatApi);
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        _allMessages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();
        foreach (var item in _allMessages)
        {
            await GetPersonalChatCompanionAsync(item);
        }
    }

    private async Task GetPersonalChatCompanionAsync(PersonalChatMessageModel personalChatMessages)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var response = await _httpClientHelper.GetAsync($"Account/{personalChatMessages.AppUserId}", refreshToken, Port.UserApi);
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var companions = await response.Content.ReadFromJsonAsync<AppUserModel>();
        personalChatMessages.Username = companions?.Username;
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
    }
}
