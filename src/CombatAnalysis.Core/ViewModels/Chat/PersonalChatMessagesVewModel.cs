using CombatAnalysis.Core.Consts;
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
                SelectedChatName = MyAccount.Id == value.InitiatorId ? value.CompanionUsername : value.InitiatorUsername;

                AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    Messages.Clear();
                });

                Task.Run(LoadMessagesAsync);
                AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
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
            if (item.PersonalChatId == SelectedChat?.Id
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
            Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}"),
            Username = MyAccount.Email,
            PersonalChatId = SelectedChat.Id,
        };

        Message = string.Empty;

        _httpClientHelper.BaseAddress = Port.ChatApi;
        await _httpClientHelper.PostAsync("PersonalChatMessage", JsonContent.Create(newPersonalChatMessage));

        SelectedChat.LastMessage = newPersonalChatMessage.Message;

        await _httpClientHelper.PutAsync("PersonalChat", JsonContent.Create(SelectedChat));
    }

    private void InitLoadMessages(object obj)
    {
        Task.Run(LoadMessagesAsync);
        AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            Fill();
        });

        _messagesUpdateTimer.Change(MessagesUpdateTimeIsMs, Timeout.Infinite);
    }

    private async Task LoadMessagesAsync()
    {
        _httpClientHelper.BaseAddress = Port.ChatApi;

        var response = await _httpClientHelper.GetAsync($"PersonalChatMessage/findByChatId/{SelectedChat?.Id}");
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return;
        }

        _allMessages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>("account");
    }
}
