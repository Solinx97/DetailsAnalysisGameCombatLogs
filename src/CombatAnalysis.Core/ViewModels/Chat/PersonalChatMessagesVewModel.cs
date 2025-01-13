using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class PersonalChatMessagesVewModel : MvxViewModel, IImprovedMvxViewModel
{
    private const string HubURL = "https://localhost:7026/personalChatHub";

    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;

    private ObservableCollection<PersonalChatMessageModel>? _messages;
    private IEnumerable<PersonalChatMessageModel>? _allMessages;
    private PersonalChatModel? _selectedChat;
    private string? _selectedChatName;
    private string? _message;
    private AppUserModel? _myAccount;
    private HubConnection? _hubConnection;

    public PersonalChatMessagesVewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        Handler = new VMHandler<PersonalChatMessagesVewModel>();
        Parent = this;
        SavedViewModel = this;

        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        SendMessageCommand = new MvxAsyncCommand(SendMessageAsync);

        Messages = [];
        _allMessages = [];

        GetMyAccount();
    }

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    #region Commands

    public IMvxAsyncCommand SendMessageCommand { get; set; }

    #endregion

    #region View model properties

    public ObservableCollection<PersonalChatMessageModel>? Messages
    {
        get { return _messages; }
        set
        {
            SetProperty(ref _messages, value);
        }
    }

    public PersonalChatModel? SelectedChat
    {
        get { return _selectedChat; }
        set
        {
            SetProperty(ref _selectedChat, value);

            if (value != null)
            {
                Task.Run(LoadMessagesForSelectedChatAsync);
                Task.Run(InitSignalRAsync);
            }
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

    public AppUserModel? MyAccount
    {
        get { return _myAccount; }
        set
        {
            SetProperty(ref _myAccount, value);
        }
    }



    #endregion

    public override void ViewDestroy(bool viewFinishing = true)
    {
        if (_hubConnection != null)
        {
            Task.Run(async () => await _hubConnection.SendAsync("LeaveFromRoom", SelectedChat?.Id.ToString()));
            Task.Run(async () => await _hubConnection.StopAsync());
        }

        base.ViewDestroy(viewFinishing);
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
            else if (_hubConnection == null)
            {
                throw new ArgumentNullException(nameof(_hubConnection));
            }

            await _hubConnection.SendAsync("SendMessage", Message, SelectedChat.Id, MyAccount.Id, MyAccount.Username);

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

    private async Task LoadMessagesForSelectedChatAsync()
    {
        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            Messages?.Clear();
        });

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

            var response = await _httpClientHelper.GetAsync($"PersonalChatMessage/getByChatId?chatId={SelectedChat?.Id}&pageSize=20", refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            _allMessages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();
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

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User)) ?? new AppUserModel();
    }

    private async Task InitSignalRAsync()
    {
        try
        {
            var cookieContainer = new CookieContainer();
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var accessToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.AccessToken));
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            cookieContainer.Add(new Uri(HubURL), new Cookie(nameof(MemoryCacheValue.RefreshToken), refreshToken));
            cookieContainer.Add(new Uri(HubURL), new Cookie(nameof(MemoryCacheValue.AccessToken), accessToken));

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(HubURL, options =>
                {
                    options.Cookies = cookieContainer;
                })
                .Build();

            await _hubConnection.StartAsync();

            await _hubConnection.SendAsync("JoinRoom", SelectedChat?.Id.ToString());

            _hubConnection.On<PersonalChatMessageModel>("ReceiveMessage", async (message) =>
            {
                await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    Messages?.Add(message);
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
