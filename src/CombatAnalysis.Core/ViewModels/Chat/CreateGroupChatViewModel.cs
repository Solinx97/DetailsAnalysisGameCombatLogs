using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class CreateGroupChatViewModel : MvxViewModel
{
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMemoryCache _memoryCache;

    private string _name;
    private int _whoCanInvitePeople;
    private int _whoCanRemovePeople;
    private int _whoCanPinMessage = 1;
    private int _whoCanMakeAnounces = 1;
    private GroupChatModel _groupChat;

    public CreateGroupChatViewModel()
    {
        _httpClientHelper = Mvx.IoCProvider.GetSingleton<IHttpClientHelper>();
        _memoryCache = Mvx.IoCProvider.GetSingleton<IMemoryCache>();

        CreateCommand = new MvxAsyncCommand(CreateAsync);
        GetWhoCanInvitePeopleCommand = new MvxCommand<int>((type) => _whoCanInvitePeople = type);
        GetWhoCanRemovePeopleCommand = new MvxCommand<int>((type) => _whoCanRemovePeople = type);
        GetWhoCanPinMessageCommand = new MvxCommand<int>((type) => _whoCanPinMessage = type);
        GetWhoCanMakeAnouncesCommand = new MvxCommand<int>((type) => _whoCanMakeAnounces = type);

        _groupChat = new GroupChatModel();
    }

    public CreateGroupChatViewModel(GroupChatModel chatModel) : this()
    {
        _groupChat = chatModel;
    }

    #region Commands

    public IMvxAsyncCommand CreateCommand { get; set; }

    public IMvxCommand CancelCommand { get; set; }

    public IMvxCommand GetWhoCanInvitePeopleCommand { get; set; }

    public IMvxCommand GetWhoCanRemovePeopleCommand { get; set; }

    public IMvxCommand GetWhoCanPinMessageCommand { get; set; }

    public IMvxCommand GetWhoCanMakeAnouncesCommand { get; set; }

    #endregion

    #region Properties

    public string Name
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
        var customer = _memoryCache.Get<CustomerModel>(nameof(MemoryCacheValue.Customer));
        if (customer == null)
        {
            return;
        }

        UpdateGroupChatModel(customer);

        var response = await CreateGroupChatAsync();
        if (response.IsSuccessStatusCode)
        {
            var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();

            await CreateGroupChatRulesAsync(groupChat.Id);
            await CreateGroupChatUserAsync(groupChat.Id, customer.Id, customer.Username);

            CancelCommand.Execute();
        }
    }

    private void UpdateGroupChatModel(CustomerModel customer)
    {
        _groupChat.Name = string.IsNullOrEmpty(Name) ? " " : Name;
        _groupChat.LastMessage = " ";
        _groupChat.CustomerId = customer.Id;
    }

    private async Task<HttpResponseMessage> CreateGroupChatAsync()
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return null;
        }

        var response = await _httpClientHelper.PostAsync("GroupChat", JsonContent.Create(_groupChat), refreshToken, Port.ChatApi);
        return response;
    }

    private async Task<HttpResponseMessage> CreateGroupChatRulesAsync(int groupChatId)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return null;
        }

        var groupChatUser = new GroupChatRulesModel
        {
            GroupChatId = groupChatId,
            Announcements = _whoCanMakeAnounces,
            InvitePeople = _whoCanInvitePeople,
            RemovePeople = _whoCanRemovePeople,
            PinMessage = _whoCanPinMessage,
        };

        var response = await _httpClientHelper.PostAsync("GroupChatRules", JsonContent.Create(groupChatUser), refreshToken, Port.ChatApi);
        return response;
    }

    private async Task CreateGroupChatUserAsync(int groupChatId, string customerId, string username)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var groupChatUser = new GroupChatUserModel
        {
            Id = "",
            Username = username,
            GroupChatId = groupChatId,
            CustomerId = customerId,
        };

        await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser), refreshToken, Port.ChatApi);
    }
}
