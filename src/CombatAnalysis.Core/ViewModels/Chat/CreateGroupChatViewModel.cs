using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class CreateGroupChatViewModel : MvxViewModel
{
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;
    private readonly GroupChatModel _groupChat;

    private string? _name;
    private int _whoCanPinMessage = 1;
    private int _whoCanMakeAnounces = 1;
    private int _whoCanInvitePeople;
    private int _whoCanRemovePeople;

    public CreateGroupChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        CreateCommand = new MvxAsyncCommand(CreateAsync);
        GetWhoCanInvitePeopleCommand = new MvxCommand<int>((type) => _whoCanInvitePeople = type);
        GetWhoCanRemovePeopleCommand = new MvxCommand<int>((type) => _whoCanRemovePeople = type);
        GetWhoCanPinMessageCommand = new MvxCommand<int>((type) => _whoCanPinMessage = type);
        GetWhoCanMakeAnouncesCommand = new MvxCommand<int>((type) => _whoCanMakeAnounces = type);

        _groupChat = new GroupChatModel();
    }

    public event CloseCreateChatWindowEventHandler? CloseCreateChatWindow;

    #region Commands

    public IMvxAsyncCommand CreateCommand { get; set; }

    public IMvxCommand GetWhoCanInvitePeopleCommand { get; set; }

    public IMvxCommand GetWhoCanRemovePeopleCommand { get; set; }

    public IMvxCommand GetWhoCanPinMessageCommand { get; set; }

    public IMvxCommand GetWhoCanMakeAnouncesCommand { get; set; }

    #endregion

    #region View model properties

    public string? Name
    {
        get { return _name; }
        set
        {
            SetProperty(ref _name, value);
        }
    }

    #endregion

    public async Task CreateAsync()
    {
        try
        {
            var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UpdateGroupChatModel(user);

            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.PostAsync("GroupChat", JsonContent.Create(_groupChat), refreshToken, Port.ChatApi);
            response.EnsureSuccessStatusCode();

            var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
            if (groupChat == null)
            {
                throw new ArgumentNullException(nameof(groupChat));
            }

            await CreateGroupChatRulesAsync(groupChat.Id);
            await CreateGroupChatUserAsync(groupChat.Id, user.Id, user.Username);

            CloseCreateChatWindow?.Invoke();
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

    public void ClearEvents()
    {
        CloseCreateChatWindow = null;
    }

    private void UpdateGroupChatModel(AppUserModel user)
    {
        _groupChat.Name = string.IsNullOrEmpty(Name) ? " " : Name;
        _groupChat.AppUserId = user.Id;
    }

    private async Task CreateGroupChatRulesAsync(int chatId)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        var groupChatUser = new GroupChatRulesModel
        {
            ChatId = chatId,
            Announcements = _whoCanMakeAnounces,
            InvitePeople = _whoCanInvitePeople,
            RemovePeople = _whoCanRemovePeople,
            PinMessage = _whoCanPinMessage,
        };

        var response = await _httpClientHelper.PostAsync("GroupChatRules", JsonContent.Create(groupChatUser), refreshToken, Port.ChatApi);
        response.EnsureSuccessStatusCode();
    }

    private async Task CreateGroupChatUserAsync(int chatId, string userId, string username)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        var groupChatUser = new GroupChatUserModel
        {
            Id = "",
            Username = username,
            ChatId = chatId,
            AppUserId = userId,
        };

        var response = await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser), refreshToken, Port.ChatApi);
        response.EnsureSuccessStatusCode();
    }

    public delegate void CloseCreateChatWindowEventHandler();
}
