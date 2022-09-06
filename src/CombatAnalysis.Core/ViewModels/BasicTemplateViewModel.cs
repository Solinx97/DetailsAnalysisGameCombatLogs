using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class BasicTemplateViewModel : MvxViewModel, IImprovedMvxViewModel, IResponseStatusObservable
    {
        private readonly List<IResponseStatusObserver> _observers;
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientHelper _httpClient;

        private int _step;
        private Tuple<int, CombatModel> _combatInformtaion;
        private List<CombatModel> _combats;
        private bool _isAuth;
        private string _email;

        private static ResponseStatus _responseStatus;
        private static int _allowStep;

        public BasicTemplateViewModel(IViewModelConnect handler, IMvxNavigationService mvvmNavigation, IMemoryCache memoryCache, IHttpClientHelper httpClient)
        {
            Handler = handler;
            _mvvmNavigation = mvvmNavigation;
            _memoryCache = memoryCache;
            _httpClient = httpClient;

            _observers = new List<IResponseStatusObserver>();

            CloseCommand = new MvxCommand(CloseWindow);
            LoginCommand = new MvxCommand(Login);
            LogoutCommand = new MvxCommand(Logout);
            UploadCombatsCommand = new MvxCommand(UploadCombatLogs);
            GeneralAnalysisCommand = new MvxCommand(GeneralAnalysis);
            CombatCommand = new MvxCommand(DetailsSpecificalCombat);

            DamageDoneDetailsCommand = new MvxCommand(DamageDoneDetails);
            HealDoneDetailsCommand = new MvxCommand(HealDoneDetails);
            DamageTakenDetailsCommand = new MvxCommand(DamageTakenDetails);
            ResourceDetailsCommand = new MvxCommand(ResourceDetails);

            CheckAuth();
        }

        public Action Close { get; set; }

        public IMvxCommand CloseCommand { get; set; }

        public IMvxCommand LoginCommand { get; set; }

        public IMvxCommand LogoutCommand { get; set; }

        public IMvxCommand RegistrationCommand { get; set; }

        public IMvxCommand UploadCombatsCommand { get; set; }

        public IMvxCommand GeneralAnalysisCommand { get; set; }

        public IMvxCommand CombatCommand { get; set; }

        public IMvxCommand DamageDoneDetailsCommand { get; set; }

        public IMvxCommand HealDoneDetailsCommand { get; set; }

        public IMvxCommand DamageTakenDetailsCommand { get; set; }

        public IMvxCommand ResourceDetailsCommand { get; set; }

        public CombatModel TargetCombat { get; set; }

        public IViewModelConnect Handler { get; set; }

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

        public ResponseStatus ResponseStatus
        {
            get { return _responseStatus; }
            set
            {
                SetProperty(ref _responseStatus, value);
                NotifyObservers();
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

        public bool IsAuth
        {
            get { return _isAuth; }
            set
            {
                SetProperty(ref _isAuth, value);
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
            }
        }

        public void CloseWindow()
        {
            WindowCloser.MainWindow.Close();
        }

        public void Login()
        {
            Task.Run(() => _mvvmNavigation.Navigate<LoginViewModel>());
        }

        public void Logout()
        {
            var refreshToken = _memoryCache.Get<string>("refreshToken");
            Task.Run(async () => await _httpClient.GetAsync($"Account/logout/{refreshToken}"));

            _memoryCache.Remove("refreshToken");
            _memoryCache.Remove("accessToken");
            _memoryCache.Remove("user");

            IsAuth = false;
            Email = string.Empty;
        }

        public void UploadCombatLogs()
        {
            Task.Run(() => _mvvmNavigation.Navigate<MainInformationViewModel>());
        }

        public void GeneralAnalysis()
        {
            Task.Run(() => _mvvmNavigation.Navigate<GeneralAnalysisViewModel, List<CombatModel>>(Combats));
        }

        public void DetailsSpecificalCombat()
        {
            Task.Run(() => _mvvmNavigation.Navigate<DetailsSpecificalCombatViewModel, CombatModel>(TargetCombat));
        }

        public void DamageDoneDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)Handler.Data;

            Task.Run(() => _mvvmNavigation.Navigate<DamageDoneDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void HealDoneDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)Handler.Data;

            Task.Run(() => _mvvmNavigation.Navigate<HealDoneDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void DamageTakenDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)Handler.Data;

            Task.Run(() => _mvvmNavigation.Navigate<DamageTakenDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void ResourceDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)Handler.Data;

            Task.Run(() => _mvvmNavigation.Navigate<ResourceRecoveryDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void AddObserver(IResponseStatusObserver o)
        {
            _observers.Add(o);
        }

        public void RemoveObserver(IResponseStatusObserver o)
        {
            _observers.Remove(o);
        }

        public void NotifyObservers()
        {
            foreach (var item in _observers)
            {
                item.Update(ResponseStatus);
            }
        }

        private void CheckAuth()
        {
            var accessToken = _memoryCache.Get<string>("accessToken");
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var user = _memoryCache.Get<UserModel>("user");
                IsAuth = true;
                Email = user.Email;
            }
        }
    }
}