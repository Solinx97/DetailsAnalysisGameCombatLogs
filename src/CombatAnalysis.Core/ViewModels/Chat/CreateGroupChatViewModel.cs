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
    private GroupChatModel _groupChat;

    public CreateGroupChatViewModel()
    {
        _httpClientHelper = Mvx.IoCProvider.GetSingleton<IHttpClientHelper>();
        _memoryCache = Mvx.IoCProvider.GetSingleton<IMemoryCache>();

        CreateCommand = new MvxAsyncCommand(CreateAsync);

        _groupChat = new GroupChatModel();
    }

    public CreateGroupChatViewModel(GroupChatModel chatModel) : this()
    {
        _groupChat = chatModel;
    }

    #region Commands

    public IMvxAsyncCommand CreateCommand { get; set; }

    public IMvxCommand CancelCommand { get; set; }

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
        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        if (user != null)
        {
            UpdateGroupChatModel(user);

            var response = await CreateGroupChatAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
                await CreateGroupChatUserAsync(groupChat.Id, user.Id);

                CancelCommand.Execute();
            }
        }
    }

    private void UpdateGroupChatModel(AppUserModel user)
    {
        _groupChat.Name = string.IsNullOrEmpty(Name) ? " " : Name;
        _groupChat.LastMessage = " ";
        _groupChat.CustomerId = user.Id;
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

    private async Task CreateGroupChatUserAsync(int groupChatId, string userId)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var groupChatUser = new GroupChatUserModel
        {
            GroupChatId = groupChatId,
            CustomerId = userId,
        };

        await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser), refreshToken, Port.ChatApi);
    }
}
