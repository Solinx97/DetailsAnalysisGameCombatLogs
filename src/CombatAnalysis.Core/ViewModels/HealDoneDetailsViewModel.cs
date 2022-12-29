using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class HealDoneDetailsViewModel : GenericTemplate<CombatPlayerModel>
{
    private readonly CombatParserAPIService _combatParserAPIService;
    private readonly PowerUpInCombat<HealDoneModel> _powerUpInCombat;

    private ObservableCollection<HealDoneModel> _healDoneInformations;
    private ObservableCollection<HealDoneModel> _healDoneInformationsWithoutFilter;
    private ObservableCollection<HealDoneModel> _healDoneInformationsWithOverheal;
    private ObservableCollection<string> _healDoneSources;
    private ObservableCollection<HealDoneGeneralModel> _healDoneGeneralInformations;

    private bool _isShowOverheal = true;
    private bool _isShowCrit = true;

    public HealDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
    {
        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);
        _powerUpInCombat = new PowerUpInCombat<HealDoneModel>(_healDoneInformationsWithOverheal);

        BasicTemplate = Templates.Basic;
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 4);
    }

    #region Properties

    public ObservableCollection<HealDoneModel> HealDoneInformations
    {
        get { return _healDoneInformations; }
        set
        {
            SetProperty(ref _healDoneInformations, value);
        }
    }

    public ObservableCollection<string> HealDoneSources
    {
        get { return _healDoneSources; }
        set
        {
            SetProperty(ref _healDoneSources, value);
        }
    }

    public ObservableCollection<HealDoneGeneralModel> HealDoneGeneralInformations
    {
        get { return _healDoneGeneralInformations; }
        set
        {
            SetProperty(ref _healDoneGeneralInformations, value);
        }
    }

    public bool IsShowOverheal
    {
        get { return _isShowOverheal; }
        set
        {
            SetProperty(ref _isShowOverheal, value);

            _powerUpInCombat.UpdateProperty("IsFullOverheal");
            _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
            HealDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", HealDoneInformations, value);

            RaisePropertyChanged(() => HealDoneInformations);
        }
    }

    public bool IsShowCrit
    {
        get { return _isShowCrit; }
        set
        {
            SetProperty(ref _isShowCrit, value);

            _powerUpInCombat.UpdateProperty("IsCrit");
            _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
            HealDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", HealDoneInformations, value);

            RaisePropertyChanged(() => HealDoneInformations);
        }
    }

    #endregion

    protected override async Task ChildPrepareAsync(CombatPlayerModel parameter)
    {
        var player = parameter;
        SelectedPlayer = player.UserName;
        TotalValue = player.HealDone;

        await LoadDetailsAsync(player.Id);
        await LoadGenericDetailsAsync(player.Id);
    }

    protected override void Filter()
    {
        HealDoneInformations = _healDoneInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<HealDoneModel>(_healDoneInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _healDoneInformationsWithoutFilter;
    }

    protected override async Task LoadDetailsAsync(int combatPlayerId)
    {
        var details = await _combatParserAPIService.LoadHealDoneDetailsAsync(combatPlayerId);
        HealDoneInformations = new ObservableCollection<HealDoneModel>(details.ToList());

        GetDetails();
    }

    protected override async Task LoadGenericDetailsAsync(int combatPlayerId)
    {
        var generalDetails = await _combatParserAPIService.LoadHealDoneGeneralAsync(combatPlayerId);
        HealDoneGeneralInformations = new ObservableCollection<HealDoneGeneralModel>(generalDetails.ToList());
    }

    protected override void GetDetails()
    {
        _healDoneInformationsWithoutFilter = new ObservableCollection<HealDoneModel>(HealDoneInformations);
        _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(HealDoneInformations);

        var sources = HealDoneInformations.Select(x => x.SpellOrItem).Distinct().ToList();
        var allSourcesName = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.HealDoneDetails.Resource.All"];
        sources.Insert(0, allSourcesName);
        HealDoneSources = new ObservableCollection<string>(sources);
    }

    protected override void TurnOnAllFilters()
    {
        if (!IsShowOverheal) IsShowOverheal = true;
        if (!IsShowCrit) IsShowCrit = true;
    }
}
