using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class BasicTemplateViewModel : MvxViewModel
    {
        private int _step;
        private Tuple<int, CombatModel> _combatInformtaion;
        private List<CombatModel> _combats;
        private IViewModelConnect _handler;
        private IMvxNavigationService _mvvmNavigation;
        private MvxViewModel _parent;

        private static bool _serverStatusIsFailed;
        private static int _allowStep;

        public BasicTemplateViewModel(MvxViewModel parent, IViewModelConnect handler, IMvxNavigationService mvvmNavigation)
        {
            _parent = parent;
            _handler = handler;
            _mvvmNavigation = mvvmNavigation;

            CloseCommand = new MvxCommand(CloseWindow);
            UploadCombatsCommand = new MvxCommand(UploadCombatLogs);
            CombatsCommand = new MvxCommand(UploadCombatLogs);
            CombatCommand = new MvxCommand(UploadCombatLogs);

            DamageDoneDetailsCommand = new MvxCommand(DamageDoneDetails);
            HealDoneDetailsCommand = new MvxCommand(HealDoneDetails);
            DamageTakenDetailsCommand = new MvxCommand(DamageTakenDetails);
            ResourceDetailsCommand = new MvxCommand(ResourceDetails);
        }

        public Action Close { get; set; }

        public IMvxCommand CloseCommand { get; set; }

        public IMvxCommand UploadCombatsCommand { get; set; }

        public IMvxCommand CombatsCommand { get; set; }

        public IMvxCommand CombatCommand { get; set; }

        public IMvxCommand DamageDoneDetailsCommand { get; set; }

        public IMvxCommand HealDoneDetailsCommand { get; set; }

        public IMvxCommand DamageTakenDetailsCommand { get; set; }

        public IMvxCommand ResourceDetailsCommand { get; set; }

        public int Step
        {
            get { return _step; }
            set
            {
                SetProperty(ref _step, value);
            }
        }

        public int AllowStep
        {
            get { return _allowStep; }
            set
            {
                SetProperty(ref _allowStep, value);
            }
        }

        public bool ServerStatusIsFailed
        {
            get { return _serverStatusIsFailed; }
            set
            {
                SetProperty(ref _serverStatusIsFailed, value);
            }
        }

        public List<CombatModel> Combats
        {
            get { return _combats; }
            set
            {
                SetProperty(ref _combats, value);
            }
        }

        public void CloseWindow()
        {
            WindowCloser.MainWindow.Close();
        }

        public void UploadCombatLogs()
        {
            Task.Run(() => _mvvmNavigation.Navigate<MainInformationViewModel>());
        }

        public void DamageDoneDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)_handler.Data;

            Task.Run(() => _mvvmNavigation.Navigate<DamageDoneDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void HealDoneDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)_handler.Data;

            Task.Run(() => _mvvmNavigation.Navigate<HealDoneDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void DamageTakenDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)_handler.Data;

            Task.Run(() => _mvvmNavigation.Navigate<DamageTakenDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void ResourceDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)_handler.Data;

            Task.Run(() => _mvvmNavigation.Navigate<ResourceRecoveryDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }
    }
}