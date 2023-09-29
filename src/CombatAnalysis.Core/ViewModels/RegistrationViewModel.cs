using CombatAnalysis.Core.Consts;
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

public class RegistrationViewModel : ParentTemplate
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMvxNavigationService _mvvmNavigation;

    private string _email;
    private string _phoneNumber;
    private DateTimeOffset _birthday = DateTimeOffset.UtcNow;
    private string _username;
    private string _firstName;
    private string _lastName;
    private string _aboutMe;
    private int _gender;
    private string _password;
    private string _confirmPassword;
    private bool _confirmPasswordIsNotCorrect;
    private bool _inputDataIsEmpty;
    private bool _accountIsReady;
    private bool _serverIsNotAvailable;

    public RegistrationViewModel(IMemoryCache memoryCache, IHttpClientHelper httpClient, IMvxNavigationService mvvmNavigation)
    {
        _memoryCache = memoryCache;
        _httpClientHelper = httpClient;
        _mvvmNavigation = mvvmNavigation;

        RegistrationCommand = new MvxAsyncCommand(ValidateAsync);
        CancelCommand = new MvxAsyncCommand(CancelAsync);

        if (BasicTemplate.Parent is LoginViewModel)
        {
            _mvvmNavigation.Close(BasicTemplate.Parent).GetAwaiter().GetResult();
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsLoginNotActivated", true);
        BasicTemplate.Parent = this;
    }

    #region Commands

    public IMvxAsyncCommand RegistrationCommand { get; set; }

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

    public string PhoneNumber
    {
        get { return _phoneNumber; }
        set
        {
            SetProperty(ref _phoneNumber, value);
        }
    }

    public DateTimeOffset Birthday
    {
        get { return _birthday; }
        set
        {
            SetProperty(ref _birthday, value);
        }
    }

    public string Username
    {
        get { return _username; }
        set
        {
            SetProperty(ref _username, value);
        }
    }

    public string FirstName
    {
        get { return _firstName; }
        set
        {
            SetProperty(ref _firstName, value);
        }
    }

    public string LastName
    {
        get { return _lastName; }
        set
        {
            SetProperty(ref _lastName, value);
        }
    }

    public string AboutMe
    {
        get { return _aboutMe; }
        set
        {
            SetProperty(ref _aboutMe, value);
        }
    }

    public int Gender
    {
        get { return _gender; }
        set
        {
            SetProperty(ref _gender, value);
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

    public string ConfirmPassword
    {
        get { return _confirmPassword; }
        set
        {
            SetProperty(ref _confirmPassword, value);
        }
    }

    public bool InputDataIsEmpty
    {
        get { return _inputDataIsEmpty; }
        set
        {
            SetProperty(ref _inputDataIsEmpty, value);
        }
    }

    public bool AccountIsReady
    {
        get { return _accountIsReady; }
        set
        {
            SetProperty(ref _accountIsReady, value);
        }
    }

    public bool ConfirmPasswordIsNotCorrect
    {
        get { return _confirmPasswordIsNotCorrect; }
        set
        {
            SetProperty(ref _confirmPasswordIsNotCorrect, value);
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

    public async Task ValidateAsync()
    {
        if (string.IsNullOrWhiteSpace(Email)
            || string.IsNullOrWhiteSpace(Password)
            || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            InputDataIsEmpty = true;
        }
        else
        {
            InputDataIsEmpty = false;
            await CheckConfirmPasswordAsync();
        }
    }

    public async Task CancelAsync()
    {
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsRegistrationNotActivated", true);
        BasicTemplate.Parent = null;
        await _mvvmNavigation.Close(this);
    }

    private async Task CheckConfirmPasswordAsync()
    {
        if (Password.Equals(ConfirmPassword))
        {
            ConfirmPasswordIsNotCorrect = false;
            ServerIsNotAvailable = false;
            AccountIsReady = false;
            InputDataIsEmpty = false;

            await RegistrationAsync();
        }
        else
        {
            ConfirmPasswordIsNotCorrect = true;
        }
    }

    private async Task RegistrationAsync()
    {
        try
        {
            var registrationModel = new RegistrationModel 
            { 
                Email = Email, 
                Password = Password,
                PhoneNumber = PhoneNumber,
                Birthday = Birthday
            };

            _httpClientHelper.BaseAddress = Port.UserApi;
            var responseMessage = await _httpClientHelper.PostAsync("Account/registration", JsonContent.Create(registrationModel));
            if (!responseMessage.IsSuccessStatusCode)
            {
                AccountIsReady = true;
                return;
            }

            var account = await responseMessage.Content.ReadFromJsonAsync<ResponseFromAccount>();

            await CreateCustomerAsync(account.User.Id, account.RefreshToken);

            _memoryCache.Set("refreshToken", account.RefreshToken, new MemoryCacheEntryOptions { Size = 10 });
            _memoryCache.Set("user", account.User, new MemoryCacheEntryOptions { Size = 50 });

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsAuth", true);
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Email", account.User.Email);

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsRegistrationNotActivated", true);
            if (BasicTemplate.Parent is LoginViewModel)
            {
                await _mvvmNavigation.Close(BasicTemplate.Parent);
            }

            await _mvvmNavigation.Close(this);
        }
        catch (HttpRequestException)
        {
            ServerIsNotAvailable = true;
        }
    }

    private async Task CreateCustomerAsync(string userId, string refreshToken)
    {
        var newCustomer = new CustomerModel
        {
            Id = "",
            Username = Username,
            FirstName = FirstName,
            LastName = LastName,
            AboutMe = AboutMe,
            Message = " ",
            Gender = Gender,
            AppUserId = userId
        };

        await _httpClientHelper.PostAsync("Customer", JsonContent.Create(newCustomer), refreshToken, Port.UserApi);
    }
}
