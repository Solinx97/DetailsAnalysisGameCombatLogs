using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class ResourceRecoveryDetailsViewModel : MvxViewModel<Tuple<int, CombatModel>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly CombatParserAPIService _combatParserAPIService;

        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<ResourceRecoveryModel> _resourceRecoveryInformations;
        private ObservableCollection<ResourceRecoveryModel> _resourceRecoveryInformationsWithoutFilter;
        private ObservableCollection<string> _resourceRecoverySources;
        private ObservableCollection<ResourceRecoveryGeneralModel> _resourceRecoveryGeneralInformations;
        private string _selectedResourceRecoverySource = "Все";
        private string _selectedPlayer;
        private double _totalValue;

        public ResourceRecoveryDetailsViewModel(IMapper mapper, IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _logger = logger;

            _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 6);
        }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<ResourceRecoveryModel> ResourceRecoveryInformations
        {
            get { return _resourceRecoveryInformations; }
            set
            {
                SetProperty(ref _resourceRecoveryInformations, value);
            }
        }

        public ObservableCollection<string> ResourceRecoverySources
        {
            get { return _resourceRecoverySources; }
            set
            {
                SetProperty(ref _resourceRecoverySources, value);
            }
        }

        public ObservableCollection<ResourceRecoveryGeneralModel> ResourceRecoveryGeneralInformations
        {
            get { return _resourceRecoveryGeneralInformations; }
            set
            {
                SetProperty(ref _resourceRecoveryGeneralInformations, value);
            }
        }

        public string SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                SetProperty(ref _selectedPlayer, value);
            }
        }

        public string SelectedResourceRecoverySource
        {
            get { return _selectedResourceRecoverySource; }
            set
            {
                SetProperty(ref _selectedResourceRecoverySource, value);

                ResourceRecoveryInformationFilter();
            }
        }

        public double TotalValue
        {
            get { return _totalValue; }
            set
            {
                SetProperty(ref _totalValue, value);
            }
        }

        public override void Prepare(Tuple<int, CombatModel> parameter)
        {
            var combat = parameter.Item2;
            var player = combat.Players[parameter.Item1];
            SelectedPlayer = player.UserName;
            TotalValue = player.EnergyRecovery;

            if (player.Id > 0)
            {
                Task.Run(async () => await LoadResourceRecoveryDetails(player.Id));
                Task.Run(async () => await LoadResourceRecoveryGeneral(player.Id));
            }
            else
            {
                CombatDetailsTemplate combatInformation = new CombatDetailsResourceRecovery(_logger);
                var map = _mapper.Map<Combat>(combat);

                GetResourceRecoveryDetails(combatInformation, SelectedPlayer, map);
                GetResourceRecoveryGeneral(combatInformation, map);
            }
        }

        private void GetResourceRecoveryDetails(CombatDetailsTemplate combatInformation, string player, Combat combat)
        {
            combatInformation.GetData(player, combat.Data);

            var map1 = _mapper.Map<ObservableCollection<ResourceRecoveryModel>>(combatInformation.ResourceRecovery);

            ResourceRecoveryInformations = map1;
            _resourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(map1);
            _resourceRecoveryInformationsWithoutFilter = new ObservableCollection<ResourceRecoveryModel>(map1);

            var resourceRecoverySources = ResourceRecoveryInformations.Select(x => x.SpellOrItem).Distinct().ToList();
            resourceRecoverySources.Insert(0, "Все");
            ResourceRecoverySources = new ObservableCollection<string>(resourceRecoverySources);
        }

        private void GetResourceRecoveryGeneral(CombatDetailsTemplate combatInformation, Combat combat)
        {
            var resourceRecoveryGeneralInformations = combatInformation.GetResourceRecoveryGeneral(combatInformation.ResourceRecovery, combat);
            var map2 = _mapper.Map<ObservableCollection<ResourceRecoveryGeneralModel>>(resourceRecoveryGeneralInformations);
            ResourceRecoveryGeneralInformations = map2;
        }

        private async Task LoadResourceRecoveryDetails(int combatPlayerId)
        {
            var resourceRecoveries = await _combatParserAPIService.LoadResourceRecoveryDetailsAsync(combatPlayerId);
            ResourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(resourceRecoveries.ToList());
            _resourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(resourceRecoveries.ToList());
        }

        private async Task LoadResourceRecoveryGeneral(int combatPlayerId)
        {
            var resourceRecoveries = await _combatParserAPIService.LoadResourceRecoveryGeneralAsync(combatPlayerId);
            ResourceRecoveryGeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(resourceRecoveries.ToList());
        }

        private void ResourceRecoveryInformationFilter()
        {
            if (_resourceRecoveryInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedResourceRecoverySource))
            {
                ResourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(_resourceRecoveryInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedResourceRecoverySource));
            }
            else
            {
                ResourceRecoveryInformations = _resourceRecoveryInformationsWithoutFilter;
            }
        }
    }
}
