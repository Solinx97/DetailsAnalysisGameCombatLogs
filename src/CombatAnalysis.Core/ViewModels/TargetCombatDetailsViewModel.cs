using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace CombatAnalysis.Core.ViewModels
{
    public class TargetCombatDetailsViewModel : MvxViewModel<CombatModel>
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IViewModelConnect _handler;

        private CombatModel _combat;
        private MvxViewModel _basicTemplate;
        private List<PlayerCombatModel> _playersCombatData;
        private PlayerCombatModel _selectedPlayer;
        private long _maxDamageDone;
        private long _maxHealDone;
        private double _maxEnergyRecovery;

        public TargetCombatDetailsViewModel(IMvxNavigationService mvvmNavigation)
        {
            _mvvmNavigation = mvvmNavigation;

            _playersCombatData = new List<PlayerCombatModel>();

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 2);
        }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public List<PlayerCombatModel> PlayersCombatData
        {
            get { return _playersCombatData; }
            set
            {
                SetProperty(ref _playersCombatData, value);
            }
        }

        public PlayerCombatModel SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                SetProperty(ref _selectedPlayer, value);
                _handler.Data = Tuple.Create(_selectedPlayer.UserName, _combat);
            }
        }

        public long MaxDamageDone
        {
            get { return _maxDamageDone; }
            set
            {
                SetProperty(ref _maxDamageDone, value);
            }
        }

        public long MaxHealDone
        {
            get { return _maxHealDone; }
            set
            {
                SetProperty(ref _maxHealDone, value);
            }
        }

        public double MaxEnergyRecovery
        {
            get { return _maxEnergyRecovery; }
            set
            {
                SetProperty(ref _maxEnergyRecovery, value);
            }
        }

        public override void Prepare(CombatModel parameter)
        {
            _combat = parameter;

            PlayersCombatData = _combat.Players;
            MaxDamageDone = _combat.DamageDone;
            MaxHealDone = _combat.HealDone;
            MaxEnergyRecovery = _combat.EnergyRecovery;
        }
    }
}
