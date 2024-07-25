using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class HealDoneDetailsViewModel : DetailsGenericTemplate<HealDoneModel, HealDoneGeneralModel>
{
    private readonly PowerUpInCombat<HealDoneModel> _powerUpInCombat;

    private ObservableCollection<HealDoneModel> _healDoneInformationsWithoutFilter;
    private ObservableCollection<HealDoneModel> _healDoneInformationsWithOverheal;

    private bool _isShowOverheal = true;
    private bool _isShowCrit = true;

    public HealDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache, IMapper mapper) : base(httpClient, logger, memoryCache, mapper)
    {
        _powerUpInCombat = new PowerUpInCombat<HealDoneModel>(_healDoneInformationsWithOverheal);

        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 4);
    }

    #region Properties

    public bool IsShowOverheal
    {
        get { return _isShowOverheal; }
        set
        {
            SetProperty(ref _isShowOverheal, value);

            _powerUpInCombat.UpdateProperty("IsFullOverheal");
            _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
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
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    #endregion

    protected override void ChildPrepare(CombatPlayerModel parameter)
    {
        var selectedCombatMap = _mapper.Map<Combat>(SelectedCombat);

        var healDoneDetails = new CombatDetailsHealDone(_logger);
        healDoneDetails.GetData(parameter.PlayerId, SelectedCombat.Data);

        var healDoneMap = _mapper.Map<List<HealDoneModel>>(healDoneDetails.HealDone);
        DetailsInformations = new ObservableCollection<HealDoneModel>(healDoneMap);

        var healDoneGeneralData = healDoneDetails.GetHealDoneGeneral(healDoneDetails.HealDone, selectedCombatMap);
        var healDoneGeneralMap = _mapper.Map<List<HealDoneGeneralModel>>(healDoneGeneralData);
        GeneralInformations = new ObservableCollection<HealDoneGeneralModel>(healDoneGeneralMap);
    }

    protected override void Filter()
    {
        DetailsInformations = _healDoneInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<HealDoneModel>(_healDoneInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _healDoneInformationsWithoutFilter;
    }

    protected override async Task LoadDetailsAsync(int combatPlayerId)
    {
        var details = await _combatParserAPIService.LoadCombatDetailsByCombatPlayerId<HealDoneModel>("HealDone/FindByCombatPlayerId", combatPlayerId);
        DetailsInformations = new ObservableCollection<HealDoneModel>(details.ToList());
    }

    protected override async Task LoadGenericDetailsAsync(int combatPlayerId)
    {
        var generalDetails = await _combatParserAPIService.LoadCombatDetailsByCombatPlayerId<HealDoneGeneralModel>("HealDoneGeneral/FindByCombatPlayerId", combatPlayerId);
        GeneralInformations = new ObservableCollection<HealDoneGeneralModel>(generalDetails.ToList());
    }

    protected override void SetUpFilteredCollection()
    {
        _healDoneInformationsWithoutFilter = new ObservableCollection<HealDoneModel>(DetailsInformations);
        _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(DetailsInformations);
    }

    protected override void SetTotalValue(CombatPlayerModel parameter)
    {
        TotalValue= parameter.HealDone;
    }

    protected override void TurnOnAllFilters()
    {
        if (!IsShowOverheal) IsShowOverheal = true;
        if (!IsShowCrit) IsShowCrit = true;
    }
}
