using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class HealDoneDetailsViewModel : GenericTemplate<Tuple<CombatPlayerModel, CombatModel>>
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

    protected override void ChildPrepare(Tuple<CombatPlayerModel, CombatModel> parameter)
    {
        var combat = parameter.Item2;
        var player = parameter.Item1;
        SelectedPlayer = player.UserName;
        TotalValue = player.HealDone;

        Task.Run(async () => await LoadHealDoneDetailsAsync(player.Id));
        Task.Run(async () => await LoadHealDoneGeneralAsync(player.Id));
    }

    protected override void GetDetails()
    {
        _healDoneInformationsWithoutFilter = new ObservableCollection<HealDoneModel>(HealDoneInformations);
        _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(HealDoneInformations);

        var healDOneSources = HealDoneInformations.Select(x => x.SpellOrItem).Distinct().ToList();
        healDOneSources.Insert(0, "Все");
        HealDoneSources = new ObservableCollection<string>(healDOneSources);
    }

    protected override void Filter()
    {
        HealDoneInformations = _healDoneInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<HealDoneModel>(_healDoneInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _healDoneInformationsWithoutFilter;
    }

    private async Task LoadHealDoneDetailsAsync(int combatPlayerId)
    {
        var healDones = await _combatParserAPIService.LoadHealDoneDetailsAsync(combatPlayerId);
        HealDoneInformations = new ObservableCollection<HealDoneModel>(healDones.ToList());
        _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(healDones.ToList());
    }

    private async Task LoadHealDoneGeneralAsync(int combatPlayerId)
    {
        var healDoneGenerals = await _combatParserAPIService.LoadHealDoneGeneralAsync(combatPlayerId);
        HealDoneGeneralInformations = new ObservableCollection<HealDoneGeneralModel>(healDoneGenerals.ToList());
    }
}
