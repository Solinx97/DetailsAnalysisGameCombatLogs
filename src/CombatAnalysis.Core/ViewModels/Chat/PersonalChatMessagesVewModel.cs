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

public class PersonalChatMessagesVewModel : MvxViewModel, IImprovedMvxViewModel
{
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;

    private ObservableCollection<PersonalChatMessageModel>? _messages;
    private IEnumerable<PersonalChatMessageModel>? _allMessages;
    private PersonalChatModel? _selectedChat;
    private string? _selectedChatName;
    private string? _message;
    private AppUserModel? _myAccount;

    public PersonalChatMessagesVewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        Handler = new VMHandler<PersonalChatMessagesVewModel>();
        Parent = this;
        SavedViewModel = this;

        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        SendMessageCommand = new MvxAsyncCommand(SendMessageAsync);

        Messages = new ObservableCollection<PersonalChatMessageModel>();
        _allMessages = new List<PersonalChatMessageModel>();

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

    public void Fill()
    {
        AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
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

    public async Task SendMessageAsync()
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
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.PostAsync("PersonalChatMessage", JsonContent.Create(newPersonalChatMessage), refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            SelectedChat.LastMessage = newPersonalChatMessage.Message;

            response = await _httpClientHelper.PutAsync("PersonalChat", JsonContent.Create(SelectedChat), refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();
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

            var tasks = new List<Task>();
            foreach (var item in _allMessages)
            {
                tasks.Add(GetPersonalChatCompanionAsync(item));
            }

            await Task.WhenAll(tasks);

            Fill();
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

    private async Task GetPersonalChatCompanionAsync(PersonalChatMessageModel message)
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(_allMessages));
            }

            var response = await _httpClientHelper.GetAsync($"Account/{message.AppUserId}", refreshToken, Port.UserApi);
            response.EnsureSuccessStatusCode();

            var companions = await response.Content.ReadFromJsonAsync<AppUserModel>();
            if (companions == null)
            {
                throw new ArgumentNullException(nameof(companions));
            }

            message.Username = companions.Username ?? string.Empty;
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
}
