using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels
{
    public class HealDoneDetailsViewModel : MvxViewModel<Tuple<string, CombatModel>>
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IViewModelConnect _handler;
        private readonly IMapper _mapper;
        private readonly PowerUpInCombat<HealDoneModel> _powerUpInCombat;

        private MvxViewModel _basicTemplate;
        private ObservableCollection<HealDoneModel> _healDoneInformations;
        private ObservableCollection<HealDoneModel> _healDoneInformationsWithOverheal;
        private ObservableCollection<HealDoneGeneralModel> _healDoneGeneralInformations;

        private bool _isShowOverheal = true;
        private bool _isShowCrit = true;
        private bool _isShowOnlyOverheal;
        private bool _isShowOnlyCrit;
        private string _selectedPlayer;

        public HealDoneDetailsViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 4);

            _powerUpInCombat = new PowerUpInCombat<HealDoneModel>(_healDoneInformationsWithOverheal);
        }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<HealDoneModel> HealDoneInformations
        {
            get { return _healDoneInformations; }
            set
            {
                SetProperty(ref _healDoneInformations, value);
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

        public bool IsShowOnlyOverheal
        {
            get { return _isShowOnlyOverheal; }
            set
            {
                SetProperty(ref _isShowOnlyOverheal, value);

                _powerUpInCombat.UpdateProperty("IsFullOverheal");
                _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
                HealDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", HealDoneInformations, value);

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

        public bool IsShowOnlyCrit
        {
            get { return _isShowOnlyCrit; }
            set
            {
                SetProperty(ref _isShowOnlyCrit, value);

                _powerUpInCombat.UpdateProperty("IsCrit");
                _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
                HealDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", HealDoneInformations, value);

                RaisePropertyChanged(() => HealDoneInformations);
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

        public override void Prepare(Tuple<string, CombatModel> parameter)
        {
            SelectedPlayer = parameter.Item1;

            GetHealDoneDetails(parameter);
        }

        private void GetHealDoneDetails(Tuple<string, CombatModel> combatInformationData)
        {
            var combatInformation = new CombatInformation();

            var map = _mapper.Map<Combat>(combatInformationData.Item2);
            combatInformation.SetCombat(map, combatInformationData.Item1);
            combatInformation.GetHealDone();

            var map1 = _mapper.Map<ObservableCollection<HealDoneModel>>(combatInformation.HealDoneInformations);

            HealDoneInformations = map1;
            _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(map1);

            var damageDoneGeneralInformations = combatInformation.GetHealDoneGeneral(combatInformation.HealDoneInformations, map);
            var map2 = _mapper.Map<ObservableCollection<HealDoneGeneralModel>>(damageDoneGeneralInformations);
            HealDoneGeneralInformations = map2;
        }
    }
}
