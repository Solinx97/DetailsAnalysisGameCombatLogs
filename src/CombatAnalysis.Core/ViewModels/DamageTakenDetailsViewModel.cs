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
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class DamageTakenDetailsViewModel : MvxViewModel<Tuple<int, CombatModel>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly PowerUpInCombat<DamageTakenModel> _powerUpInCombat;
        private readonly CombatParserAPIService _combatParserAPIService;

        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<DamageTakenModel> _damageTakenInformations;
        private ObservableCollection<DamageTakenModel> _damageTakenInformationsWithSkipDamage;
        private ObservableCollection<DamageTakenGeneralModel> _damageTakenGeneralInformations;

        private bool _isShowDodge = true;
        private bool _isShowParry = true;
        private bool _isShowMiss = true;
        private bool _isShowResist = true;
        private bool _isShowImmune = true;
        private bool _isShowCrushing = true;
        private bool _isShowAbsorb = true;
        private bool _isShowDamageInform;
        private string _selectedPlayer;
        private int _selectedIndexSorting;
        private bool _isCollectionReversed;
        private long _totalValue;

        public DamageTakenDetailsViewModel(IMapper mapper, IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _logger = logger;

            _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);
            _powerUpInCombat = new PowerUpInCombat<DamageTakenModel>(_damageTakenInformationsWithSkipDamage);

            ShowDamageInformCommand = new MvxCommand(() => IsShowDamageInfrom = !IsShowDamageInfrom);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 5);
        }

        public IMvxCommand ShowDamageInformCommand { get; set; }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<DamageTakenModel> DamageTakenInformations
        {
            get { return _damageTakenInformations; }
            set
            {
                SetProperty(ref _damageTakenInformations, value);
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

        public int SelectedIndexSorting
        {
            get { return _selectedIndexSorting; }
            set
            {
                SetProperty(ref _selectedIndexSorting, value);

                Sorting(value);

                RaisePropertyChanged(() => DamageTakenGeneralInformations);
            }
        }

        public bool IsCollectionReversed
        {
            get { return _isCollectionReversed; }
            set
            {
                SetProperty(ref _isCollectionReversed, value);

                Reverse();

                RaisePropertyChanged(() => DamageTakenGeneralInformations);
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

        public override void Prepare(Tuple<int, CombatModel> parameter)
        {
            var combat = parameter.Item2;
            var player = combat.Players[parameter.Item1];
            SelectedPlayer = player.UserName;
            TotalValue = player.DamageTaken;

            if (player.Id > 0)
            {
                Task.Run(async () => await LoadDamageTakenDetails(player.Id));
                Task.Run(async () => await LoadDamageTakenGeneral(player.Id));
            }
            else
            {
                CombatDetailsTemplate combatInformation = new CombatDetailsDamageTaken(_logger);
                var map = _mapper.Map<Combat>(combat);

                GetDamageTakenDetails(combatInformation, SelectedPlayer, map);
                GetDamageTakenGeneral(combatInformation, map);
            }
        }

        private void GetDamageTakenDetails(CombatDetailsTemplate combatInformation, string player, Combat combat)
        {
            combatInformation.GetData(player, combat.Data);

            var map1 = _mapper.Map<ObservableCollection<DamageTakenModel>>(combatInformation.DamageTaken);

            DamageTakenInformations = map1;
            _damageTakenInformationsWithSkipDamage = new ObservableCollection<DamageTakenModel>(map1);
        }

        private void GetDamageTakenGeneral(CombatDetailsTemplate combatInformation, Combat combat)
        {
            var damageDoneGeneralInformations = combatInformation.GetDamageTakenGeneral(combatInformation.DamageTaken, combat);
            var map2 = _mapper.Map<ObservableCollection<DamageTakenGeneralModel>>(damageDoneGeneralInformations);
            DamageTakenGeneralInformations = map2;
        }

        private async Task LoadDamageTakenDetails(int combatPlayerId)
        {
            var healDones = await _combatParserAPIService.LoadDamageTakenDetailsAsync(combatPlayerId);
            DamageTakenInformations = new ObservableCollection<DamageTakenModel>(healDones.ToList());
            _damageTakenInformationsWithSkipDamage = new ObservableCollection<DamageTakenModel>(healDones.ToList());
        }

        private async Task LoadDamageTakenGeneral(int combatPlayerId)
        {
            var healDoneGenerals = await _combatParserAPIService.LoadDamageTakenGeneralAsync(combatPlayerId);
            DamageTakenGeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(healDoneGenerals.ToList());
        }

        private void Sorting(int index)
        {
            IOrderedEnumerable<DamageTakenGeneralModel> sortedCollection;

            switch (index)
            {
                case 0:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.SpellOrItem);
                    break;
                case 1:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.Value);
                    break;
                case 2:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.CastNumber);
                    break;
                case 3:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.MinValue);
                    break;
                case 4:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.MaxValue);
                    break;
                case 5:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.AverageValue);
                    break;
                case 6:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.DamageTakenPerSecond);
                    break;
                default:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.Value);
                    break;
            }

            DamageTakenGeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(sortedCollection.ToList());
            IsCollectionReversed = false;
        }

        private void Reverse()
        {
            DamageTakenGeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(DamageTakenGeneralInformations.Reverse().ToList());
        }
    }
}
