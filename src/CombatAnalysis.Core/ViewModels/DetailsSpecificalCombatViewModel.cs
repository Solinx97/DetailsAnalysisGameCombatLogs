using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace CombatAnalysis.Core.ViewModels
{
    public class DetailsSpecificalCombatViewModel : MvxViewModel<CombatModel>
    {
        private CombatModel _combat;
        private IImprovedMvxViewModel _basicTemplate;
        private List<CombatPlayerModel> _playersCombatData;
        private long _maxDamageDone;
        private long _maxHealDone;
        private double _maxEnergyRecovery;
        private CombatPlayerModel _selectedPlayer;
        private string _selectedCombat;

        public DetailsSpecificalCombatViewModel()
        {
            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 2);
        }

        #region Properties

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public List<CombatPlayerModel> PlayersCombatData
        {
            get { return _playersCombatData; }
            set
            {
                SetProperty(ref _playersCombatData, value);

                SelectedPlayer = value[0];
            }
        }

        public CombatPlayerModel SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                SetProperty(ref _selectedPlayer, value);

                BasicTemplate.Handler.Data = Tuple.Create(value, _combat);
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

        public string SelectedCombat
        {
            get { return _selectedCombat; }
            set
            {
                SetProperty(ref _selectedCombat, value);
            }
        }

        #endregion

        public override void Prepare(CombatModel parameter)
        {
            _combat = parameter;

            PlayersCombatData = _combat.Players;
            MaxDamageDone = _combat.DamageDone;
            MaxHealDone = _combat.HealDone;
            MaxEnergyRecovery = _combat.EnergyRecovery;

            SelectedCombat = parameter.Name;
        }
    }
}
