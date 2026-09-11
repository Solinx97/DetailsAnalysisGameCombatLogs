using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces.Security;
using CombatAnalysis.UploadingLogsApp.Models.User;
using CombatAnalysis.UploadingLogsApp.ViewModels.User;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Caching.Memory;

namespace CombatAnalysis.UploadingLogsApp.Core;

public partial class AppState : ObservableObject
{
    private readonly IMemoryCache _memoryCache;
    private readonly ISecurityStorage _securityStorage;

    public AppState(IMemoryCache memoryCache, ISecurityStorage securityStorage)
    {
        _memoryCache = memoryCache;
        _securityStorage = securityStorage;
    }


    [ObservableProperty]
    private string appName;

    [ObservableProperty]
    private string appVersion;

    [ObservableProperty]
    private bool isAuth;

    [ObservableProperty]
    private AppUserModel? user;

    [ObservableProperty]
    private bool allowLogout = true;

    public void Logout()
    {
        User = null;
        IsAuth = false;

        _memoryCache.Remove(nameof(MemoryCacheValue.User));
        _memoryCache.Remove(nameof(MemoryCacheValue.Customer));
        _memoryCache.Remove(nameof(MemoryCacheValue.AccessToken));
        _memoryCache.Remove(nameof(MemoryCacheValue.RefreshToken));

        _securityStorage.RemoveAccessToken();
    }
}
