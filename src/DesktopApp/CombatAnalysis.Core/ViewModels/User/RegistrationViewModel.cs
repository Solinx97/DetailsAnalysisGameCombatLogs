using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;

namespace CombatAnalysis.Core.ViewModels.User;

public class RegistrationViewModel : ParentTemplate
{
    private readonly IMemoryCache _memoryCache;
    private readonly IIdentityService _identityService;
    private readonly ILogger _logger;
    private readonly CancellationTokenSource _cts = new();

    private bool _isVerification;
    private bool _isCanceled;

    public event Action<bool>? RegistrationCompleted;

    public RegistrationViewModel(IMemoryCache memoryCache, IIdentityService identityService, ILogger logger)
    {
        _memoryCache = memoryCache;
        _identityService = identityService;
        _logger = logger;

        AbortCommand = new MvxCommand(Abort);

        Basic.Parent = this;
    }

    public IMvxCommand AbortCommand { get; set; }

    #region View model properties

    public bool IsVerification
    {
        get { return _isVerification; }
        set
        {
            SetProperty(ref _isVerification, value);
        }
    }

    public bool IsCanceled
    {
        get { return _isCanceled; }
        set
        {
            SetProperty(ref _isCanceled, value);
        }
    }

    #endregion

    public override async Task Initialize()
    {
        await SendAuthorizationRequestAsync();
    }

    private async Task SendAuthorizationRequestAsync()
    {
        try
        {
            await _identityService.SendAuthorizationRequestAsync("Account/Registration", _cts.Token);
            _cts.Token.ThrowIfCancellationRequested();

            IsVerification = true;

            await _identityService.SendTokenRequestAsync(_cts.Token);
            _cts.Token.ThrowIfCancellationRequested();

            var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            if (user != null)
            {
                Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsAuth), true);
                Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Username), user.Username);
            }

            RegistrationCompleted?.Invoke(true);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Listening cancelled");
        }
    }

    private void Abort()
    {
        _cts.Cancel();

        IsVerification = false;
        IsCanceled = true;
    }
}
