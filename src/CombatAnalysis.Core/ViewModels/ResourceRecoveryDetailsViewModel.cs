using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class ResourceRecoveryDetailsViewModel : DetailsGenericTemplate<ResourceRecoveryModel, ResourceRecoveryGeneralModel>
{
    private ObservableCollection<ResourceRecoveryModel> _resourceRecoveryInformationsWithoutFilter;

    public ResourceRecoveryDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache, IMapper mapper) : base(httpClient, logger, memoryCache, mapper)
    {
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 6);
    }

    protected override void ChildPrepare(CombatPlayerModel parameter)
    {
        var selectedCombatMap = _mapper.Map<Combat>(SelectedCombat);

        var resourceRecovery = new CombatDetailsResourceRecovery(_logger);
        resourceRecovery.GetData(parameter.PlayerId, SelectedCombat.Data);

        var healDoneMap = _mapper.Map<List<ResourceRecoveryModel>>(resourceRecovery.ResourceRecovery);
        DetailsInformations = new ObservableCollection<ResourceRecoveryModel>(healDoneMap);

        var healDoneGeneralData = resourceRecovery.GetResourceRecoveryGeneral(resourceRecovery.ResourceRecovery, selectedCombatMap);
        var healDoneGeneralMap = _mapper.Map<List<ResourceRecoveryGeneralModel>>(healDoneGeneralData);
        GeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(healDoneGeneralMap);
    }

    protected override void Filter()
    {
        DetailsInformations = _resourceRecoveryInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<ResourceRecoveryModel>(_resourceRecoveryInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _resourceRecoveryInformationsWithoutFilter;
    }

    protected override async Task LoadDetailsAsync(int combatPlayerId)
    {
        var details = await _combatParserAPIService.LoadCombatDetailsByCombatPlayerId<ResourceRecoveryModel>("ResourceRecovery/FindByCombatPlayerId", combatPlayerId);
        DetailsInformations = new ObservableCollection<ResourceRecoveryModel>(details.ToList());
    }

    protected override async Task LoadGenericDetailsAsync(int combatPlayerId)
    {
        var generalDetails = await _combatParserAPIService.LoadCombatDetailsByCombatPlayerId<ResourceRecoveryGeneralModel>("ResourceRecoveryGeneral/FindByCombatPlayerId", combatPlayerId);
        GeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(generalDetails.ToList());
    }

    protected override void SetUpFilteredCollection()
    {
        _resourceRecoveryInformationsWithoutFilter = new ObservableCollection<ResourceRecoveryModel>(DetailsInformations);
    }

    protected override void SetTotalValue(CombatPlayerModel parameter)
    {
        TotalValue = parameter.EnergyRecovery;
    }

    protected override void TurnOnAllFilters()
    {
        // write here your filters
    }
}
