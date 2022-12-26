using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class DamageTakenDetailsViewModel : GenericTemplate<CombatPlayerModel>
{
    private readonly PowerUpInCombat<DamageTakenModel> _powerUpInCombat;
    private readonly CombatParserAPIService _combatParserAPIService;

    private ObservableCollection<DamageTakenModel> _damageTakenInformations;
    private ObservableCollection<DamageTakenModel> _damageTakenInformationsWithoutFilter;
    private ObservableCollection<DamageTakenModel> _damageTakenInformationsWithSkipDamage;
    private ObservableCollection<string> _damageTakenSources;
    private ObservableCollection<DamageTakenGeneralModel> _damageTakenGeneralInformations;

    private bool _isShowDodge = true;
    private bool _isShowParry = true;
    private bool _isShowMiss = true;
    private bool _isShowResist = true;
    private bool _isShowImmune = true;
    private bool _isShowCrushing = true;
    private bool _isShowAbsorb = true;
    private bool _isShowDamageInform = true;
    private bool _isShowFilters;

    public DamageTakenDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
    {
        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);
        _powerUpInCombat = new PowerUpInCombat<DamageTakenModel>(_damageTakenInformationsWithSkipDamage);

        ShowDamageInformCommand = new MvxCommand(() => IsShowDamageInfrom = !IsShowDamageInfrom);

        BasicTemplate = Templates.Basic;
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 5);
    }

    #region Commands

    public IMvxCommand ShowDamageInformCommand { get; set; }

    #endregion

    #region Properties

    public ObservableCollection<DamageTakenModel> DamageTakenInformations
    {
        get { return _damageTakenInformations; }
        set
        {
            SetProperty(ref _damageTakenInformations, value);
        }
    }

    public ObservableCollection<string> DamageTakenSources
    {
        get { return _damageTakenSources; }
        set
        {
            SetProperty(ref _damageTakenSources, value);
        }
    }

    public ObservableCollection<DamageTakenGeneralModel> DamageTakenGeneralInformations
    {
        get { return _damageTakenGeneralInformations; }
        set
        {
            SetProperty(ref _damageTakenGeneralInformations, value);
        }
    }

    public bool IsShowDodge
    {
        get { return _isShowDodge; }
        set
        {
            SetProperty(ref _isShowDodge, value);

            _powerUpInCombat.UpdateProperty("IsDodge");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

            RaisePropertyChanged(() => DamageTakenInformations);
        }
    }

    public bool IsShowParry
    {
        get { return _isShowParry; }
        set
        {
            SetProperty(ref _isShowParry, value);

            _powerUpInCombat.UpdateProperty("IsParry");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

            RaisePropertyChanged(() => DamageTakenInformations);
        }
    }

    public bool IsShowMiss
    {
        get { return _isShowMiss; }
        set
        {
            SetProperty(ref _isShowMiss, value);

            _powerUpInCombat.UpdateProperty("IsMiss");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

            RaisePropertyChanged(() => DamageTakenInformations);
        }
    }

    public bool IsShowResist
    {
        get { return _isShowResist; }
        set
        {
            SetProperty(ref _isShowResist, value);

            _powerUpInCombat.UpdateProperty("IsResist");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

            RaisePropertyChanged(() => DamageTakenInformations);
        }
    }

    public bool IsShowImmune
    {
        get { return _isShowImmune; }
        set
        {
            SetProperty(ref _isShowImmune, value);

            _powerUpInCombat.UpdateProperty("IsImmune");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

            RaisePropertyChanged(() => DamageTakenInformations);
        }
    }

    public bool IsShowCrushing
    {
        get { return _isShowCrushing; }
        set
        {
            SetProperty(ref _isShowCrushing, value);

            _powerUpInCombat.UpdateProperty("IsCrushing");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

            RaisePropertyChanged(() => DamageTakenInformations);
        }
    }

    public bool IsShowAbsorb
    {
        get { return _isShowAbsorb; }
        set
        {
            SetProperty(ref _isShowAbsorb, value);

            _powerUpInCombat.UpdateProperty("IsAbsorb");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

            RaisePropertyChanged(() => DamageTakenInformations);
        }
    }

    public bool IsShowDamageInfrom
    {
        get { return _isShowDamageInform; }
        set
        {
            SetProperty(ref _isShowDamageInform, value);

            RaisePropertyChanged(() => DamageTakenInformations);
        }
    }

    #endregion

    protected override async Task ChildPrepareAsync(CombatPlayerModel parameter)
    {
        var player = parameter;
        SelectedPlayer = player.UserName;
        TotalValue = player.DamageTaken;

        await LoadDetailsAsync(player.Id);
        await LoadGenericDetailsAsync(player.Id);
    }

    protected override void Filter()
    {
        DamageTakenInformations = _damageTakenInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<DamageTakenModel>(_damageTakenInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _damageTakenInformationsWithoutFilter;
    }

    protected override async Task LoadDetailsAsync(int combatPlayerId)
    {
        var details = await _combatParserAPIService.LoadDamageTakenDetailsAsync(combatPlayerId);
        DamageTakenInformations = new ObservableCollection<DamageTakenModel>(details.ToList());

        GetDetails();
    }

    protected override async Task LoadGenericDetailsAsync(int combatPlayerId)
    {
        var generalDetails = await _combatParserAPIService.LoadDamageTakenGeneralAsync(combatPlayerId);
        DamageTakenGeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(generalDetails.ToList());
    }

    protected override void GetDetails()
    {
        _damageTakenInformationsWithSkipDamage = new ObservableCollection<DamageTakenModel>(DamageTakenInformations);
        _damageTakenInformationsWithoutFilter = new ObservableCollection<DamageTakenModel>(DamageTakenInformations);

        var sources = DamageTakenInformations.Select(x => x.SpellOrItem).Distinct().ToList();
        var allSourcesName = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.DamageTakenDetails.Resource.All"];
        sources.Insert(0, allSourcesName);
        DamageTakenSources = new ObservableCollection<string>(sources);
    }

    protected override void TurnOnAllFilters()
    {
        if (!IsShowDodge) IsShowDodge = true;
        if (!IsShowParry) IsShowParry = true;
        if (!IsShowMiss) IsShowMiss = true;
        if (!IsShowResist) IsShowResist = true;
        if (!IsShowImmune) IsShowImmune = true;
        if (!IsShowCrushing) IsShowCrushing = true;
        if (!IsShowAbsorb) IsShowAbsorb = true;
    }
}
