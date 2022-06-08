using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.CombatParser.Models;
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
        private readonly PowerUpInCombat<HealDoneInformationModel> _powerUpInCombat;

        private MvxViewModel _basicTemplate;
        private ObservableCollection<HealDoneInformationModel> _healDoneInformations;
        private ObservableCollection<HealDoneInformationModel> _healDoneInformationsWithOverheal;

        private bool _isShowOverheal = true;
        private bool _isShowCrit = true;
        private bool _isShowOnlyOverheal;
        private bool _isShowOnlyCrit;

        public HealDoneDetailsViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 4);

            _powerUpInCombat = new PowerUpInCombat<HealDoneInformationModel>(_healDoneInformationsWithOverheal);
        }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<HealDoneInformationModel> HealDoneInformations
        {
            get { return _healDoneInformations; }
            set
            {
                SetProperty(ref _healDoneInformations, value);
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

        public override void Prepare(Tuple<string, CombatModel> parameter)
        {
            GetHealDoneDetails(parameter);
        }

        private void GetHealDoneDetails(Tuple<string, CombatModel> combatInformationData)
        {
            var combatInformation = new CombatInformation();

            var map = _mapper.Map<Combat>(combatInformationData.Item2);
            combatInformation.SetCombat(map, combatInformationData.Item1);
            combatInformation.GetHealDone();

            var map1 = _mapper.Map<ObservableCollection<HealDoneInformationModel>>(combatInformation.HealDoneInformations);

            HealDoneInformations = map1;
            _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneInformationModel>(map1);
        }
    }
}
