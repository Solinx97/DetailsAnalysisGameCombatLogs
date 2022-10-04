using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CombatAnalysis.Core.ViewModels
{
    public class DamageDoneDetailsViewModel : MvxViewModel<Tuple<CombatPlayerModel, CombatModel>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly PowerUpInCombat<DamageDoneModel> _powerUpInCombat;
        private readonly CombatParserAPIService _combatParserAPIService;

        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<DamageDoneModel> _damageDoneInformations;
        private ObservableCollection<DamageDoneModel> _damageDoneInformationsWithoutFilter;
        private ObservableCollection<DamageDoneModel> _damageDoneInformationsWithSkipDamage;
        private ObservableCollection<string> _damageDoneSources;
        private ObservableCollection<DamageDoneGeneralModel> _damageDoneGeneralInformations;

        private bool _isShowCrit = true;
        private bool _isShowDodge = true;
        private bool _isShowParry = true;
        private bool _isShowMiss = true;
        private bool _isShowResist = true;
        private bool _isShowImmune = true;
        private bool _isShowDirectDamage;
        private bool _isShowFilters;
        private string _selectedPlayer;
        private string _selectedDamageDoneSource = "Все";
        private long _totalValue;

        public DamageDoneDetailsViewModel(IMapper mapper, IHttpClientHelper httpClient, ILogger loger, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _logger = loger;

            _combatParserAPIService = new CombatParserAPIService(httpClient, loger, memoryCache);
            _powerUpInCombat = new PowerUpInCombat<DamageDoneModel>(_damageDoneInformationsWithSkipDamage);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 3);
        }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<DamageDoneModel> DamageDoneInformations
        {
            get { return _damageDoneInformations; }
            set
            {
                SetProperty(ref _damageDoneInformations, value);
            }
        }

        public ObservableCollection<string> DamageDoneSources
        {
            get { return _damageDoneSources; }
            set
            {
                SetProperty(ref _damageDoneSources, value);
            }
        }

        public ObservableCollection<DamageDoneGeneralModel> DamageDoneGeneralInformations
        {
            get { return _damageDoneGeneralInformations; }
            set
            {
                SetProperty(ref _damageDoneGeneralInformations, value);
            }
        }

        public bool IsShowCrit
        {
            get { return _isShowCrit; }
            set
            {
                SetProperty(ref _isShowCrit, value);

                _powerUpInCombat.UpdateProperty("IsCrit");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

                RaisePropertyChanged(() => DamageDoneInformations);
            }
        }

        public bool IsShowDodge
        {
            get { return _isShowDodge; }
            set
            {
                SetProperty(ref _isShowDodge, value);

                _powerUpInCombat.UpdateProperty("IsDodge");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

                RaisePropertyChanged(() => DamageDoneInformations);
            }
        }

        public bool IsShowParry
        {
            get { return _isShowParry; }
            set
            {
                SetProperty(ref _isShowParry, value);

                _powerUpInCombat.UpdateProperty("IsParry");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

                RaisePropertyChanged(() => DamageDoneInformations);
            }
        }

        public bool IsShowMiss
        {
            get { return _isShowMiss; }
            set
            {
                SetProperty(ref _isShowMiss, value);

                _powerUpInCombat.UpdateProperty("IsMiss");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

                RaisePropertyChanged(() => DamageDoneInformations);
            }
        }

        public bool IsShowResist
        {
            get { return _isShowResist; }
            set
            {
                SetProperty(ref _isShowResist, value);

                _powerUpInCombat.UpdateProperty("IsResist");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

                RaisePropertyChanged(() => DamageDoneInformations);
            }
        }

        public bool IsShowImmune
        {
            get { return _isShowImmune; }
            set
            {
                SetProperty(ref _isShowImmune, value);

                _powerUpInCombat.UpdateProperty("IsImmune");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

                RaisePropertyChanged(() => DamageDoneInformations);
            }
        }

        public bool IsShowDirectDamage
        {
            get { return _isShowDirectDamage; }
            set
            {
                SetProperty(ref _isShowDirectDamage, value);
            }
        }

        public bool IsShowFilters
        {
            get { return _isShowFilters; }
            set
            {
                SetProperty(ref _isShowFilters, value);
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

        public string SelectedDamageDoneSource
        {
            get { return _selectedDamageDoneSource; }
            set
            {
                SetProperty(ref _selectedDamageDoneSource, value);

                DamageDoneInformationFilter();
            }
        }

        public long TotalValue
        {
            get { return _totalValue; }
            set
            {
                SetProperty(ref _totalValue, value);
            }
        }

        public override void Prepare(Tuple<CombatPlayerModel, CombatModel> parameter)
        {
            var combat = parameter.Item2;
            var player = parameter.Item1;
            SelectedPlayer = player.UserName;
            TotalValue = player.DamageDone;

            if (player.Id > 0)
            {
                Task.Run(async () => await LoadDamageDoneDetails(player.Id));
                Task.Run(async () => await LoadDamageDoneGeneral(player.Id));
            }
            else
            {
                CombatDetailsTemplate combatInformation = new CombatDetailsDamageDone(_logger);
                var map = _mapper.Map<Combat>(combat);

                GetDamageDoneDetails(combatInformation, SelectedPlayer, map);
                GetDamageDoneGeneral(combatInformation, map);
            }
        }

        private void GetDamageDoneDetails(CombatDetailsTemplate combatDetails, string player, Combat combat)
        {
            combatDetails.GetData(player, combat.Data);

            var map1 = _mapper.Map<ObservableCollection<DamageDoneModel>>(combatDetails.DamageDone);

            DamageDoneInformations = map1;
            _damageDoneInformationsWithoutFilter = new ObservableCollection<DamageDoneModel>(map1);
            _damageDoneInformationsWithSkipDamage = new ObservableCollection<DamageDoneModel>(map1);

            var damageDoneSources = DamageDoneInformations.Select(x => x.SpellOrItem).Distinct().ToList();
            damageDoneSources.Insert(0, "Все");
            DamageDoneSources = new ObservableCollection<string>(damageDoneSources);
        }

        private void GetDamageDoneGeneral(CombatDetailsTemplate combatDetails, Combat combat)
        {
            var damageDoneGeneralInformations = combatDetails.GetDamageDoneGeneral(combatDetails.DamageDone, combat);
            var map2 = _mapper.Map<ObservableCollection<DamageDoneGeneralModel>>(damageDoneGeneralInformations);
            DamageDoneGeneralInformations = map2;
        }

        private async Task LoadDamageDoneDetails(int combatPlayerId)
        {
            var healDones = await _combatParserAPIService.LoadDamageDoneDetailsAsync(combatPlayerId);
            DamageDoneInformations = new ObservableCollection<DamageDoneModel>(healDones.ToList());
            _damageDoneInformationsWithSkipDamage = new ObservableCollection<DamageDoneModel>(healDones.ToList());
        }

        private async Task LoadDamageDoneGeneral(int combatPlayerId)
        {
            var healDoneGenerals = await _combatParserAPIService.LoadDamageDoneGeneralAsync(combatPlayerId);
            DamageDoneGeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(healDoneGenerals.ToList());
        }

        private void DamageDoneInformationFilter()
        {
            if (_damageDoneInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedDamageDoneSource))
            {
                DamageDoneInformations = new ObservableCollection<DamageDoneModel>(_damageDoneInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedDamageDoneSource));
            }
            else
            {
                DamageDoneInformations = _damageDoneInformationsWithoutFilter;
            }
        }
    }
}