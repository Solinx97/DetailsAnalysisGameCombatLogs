using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Response;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels;

public class LoginViewModel : ParentTemplate
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMvxNavigationService _mvvmNavigation;

    private string _email;
    private string _password;
    private bool _authIsFailed;
    private bool _serverIsNotAvailable;

    public LoginViewModel(IMemoryCache memoryCache, IHttpClientHelper httpClient, IMvxNavigationService mvvmNavigation)
    {
        _memoryCache = memoryCache;
        _httpClientHelper = httpClient;
        _mvvmNavigation = mvvmNavigation;

        LoginCommand = new MvxCommand(Login);
        CancelCommand = new MvxAsyncCommand(CancelAsync);

        if (BasicTemplate.Parent is RegistrationViewModel)
        {
            _mvvmNavigation.Close(BasicTemplate.Parent).GetAwaiter().GetResult();
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsRegistrationNotActivated", true);
        BasicTemplate.Parent = this;
    }

    #region Commands

    public IMvxCommand LoginCommand { get; set; }

    public IMvxAsyncCommand CancelCommand { get; set; }

    #endregion

    #region Properties

    public string Email
    {
        get { return _email; }
        set
        {
            SetProperty(ref _email, value);
        }
    }

    public string Password
    {
        get { return _password; }
        set
        {
            SetProperty(ref _password, value);
        }
    }

    public bool AuthIsFailed
    {
        get { return _authIsFailed; }
        set
        {
            SetProperty(ref _authIsFailed, value);
        }
    }

    public bool ServerIsNotAvailable
    {
        get { return _serverIsNotAvailable; }
        set
        {
            SetProperty(ref _serverIsNotAvailable, value);
        }
    }

    #endregion

    public void Login()
    {
        AuthIsFailed = false;
        ServerIsNotAvailable = false;

        var loginModel = new LoginModel { Email = Email, Password = Password };

        Action action = async () =>
        {
            try
            {
                var responseMessage = await _httpClientHelper.PostAsync("Account", JsonContent.Create(loginModel), Port.UserApi);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    AuthIsFailed = true;
                }

                var response = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();
                var customer = await GetCustomerAsync(response.RefreshToken, response.User.Id);

                SetMemoryCache(response.RefreshToken, response.User, customer);

                BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsAuth", true);
                BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Email", response.User.Email);

                BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsLoginNotActivated", true);

                await _mvvmNavigation.Close(this);
            }
            catch (HttpRequestException)
            {
                ServerIsNotAvailable = true;
            }
        };

        Task.Run(action);
    }

    public async Task CancelAsync()
    {
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsLoginNotActivated", true);
        BasicTemplate.Parent = null;
        await _mvvmNavigation.Close(this);
    }

    private async Task<CustomerModel> GetCustomerAsync(string refreshToken, string userId)
    {
        var response = await _httpClientHelper.GetAsync($"Customer/searchByUserId/{userId}", refreshToken, Port.UserApi);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var customer = await response.Content.ReadFromJsonAsync<IEnumerable<CustomerModel>>();
        return customer.FirstOrDefault();
    }

    private void SetMemoryCache(object refreshToken, object user, object customer)
    {
        _memoryCache.Set(nameof(MemoryCacheValue.RefreshToken), refreshToken, new MemoryCacheEntryOptions { Size = 10 });
        _memoryCache.Set(nameof(MemoryCacheValue.User), user, new MemoryCacheEntryOptions { Size = 50 });
        _memoryCache.Set(nameof(MemoryCacheValue.Customer), customer, new MemoryCacheEntryOptions { Size = 50 });
    }
}
