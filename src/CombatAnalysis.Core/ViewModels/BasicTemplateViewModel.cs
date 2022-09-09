using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
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
    public class BasicTemplateViewModel : MvxViewModel, IImprovedMvxViewModel, IResponseStatusObservable, IAuthObservable
    {
        private readonly List<IResponseStatusObserver> _responseStatusObservers;
        private readonly List<IAuthObserver> _authObservers;
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientHelper _httpClient;

        private int _step;
        private Tuple<int, CombatModel> _combatInformtaion;
        private List<CombatModel> _combats;
        private bool _isAuth;
        private string _email;
        private LogType _logType;

        private static ResponseStatus _responseStatus;
        private static int _allowStep;

        public BasicTemplateViewModel(IViewModelConnect handler, IMvxNavigationService mvvmNavigation, IMemoryCache memoryCache, IHttpClientHelper httpClient)
        {
            Handler = handler;
            _mvvmNavigation = mvvmNavigation;
            _memoryCache = memoryCache;
            _httpClient = httpClient;

            _responseStatusObservers = new List<IResponseStatusObserver>();
            _authObservers = new List<IAuthObserver>();

            CloseCommand = new MvxCommand(CloseWindow);
            LoginCommand = new MvxCommand(Login);
            RegistrationCommand = new MvxCommand(Registration);
            LogoutCommand = new MvxCommand(Logout);
            UploadCombatsCommand = new MvxCommand(UploadCombatLogs);
            GeneralAnalysisCommand = new MvxCommand(GeneralAnalysis);
            CombatCommand = new MvxCommand(DetailsSpecificalCombat);

            DamageDoneDetailsCommand = new MvxCommand(DamageDoneDetails);
            HealDoneDetailsCommand = new MvxCommand(HealDoneDetails);
            DamageTakenDetailsCommand = new MvxCommand(DamageTakenDetails);
            ResourceDetailsCommand = new MvxCommand(ResourceDetails);
        }

        public Action Close { get; set; }

        public IMvxCommand CloseCommand { get; set; }

        public IMvxCommand LoginCommand { get; set; }

        public IMvxCommand RegistrationCommand { get; set; }

        public IMvxCommand LogoutCommand { get; set; }

        public IMvxCommand UploadCombatsCommand { get; set; }

        public IMvxCommand GeneralAnalysisCommand { get; set; }

        public IMvxCommand CombatCommand { get; set; }

        public IMvxCommand DamageDoneDetailsCommand { get; set; }

        public IMvxCommand HealDoneDetailsCommand { get; set; }

        public IMvxCommand DamageTakenDetailsCommand { get; set; }

        public IMvxCommand ResourceDetailsCommand { get; set; }

        public CombatModel TargetCombat { get; set; }

        public IViewModelConnect Handler { get; set; }

        public IMvxViewModel Parent { get; set; }

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
                NotifyResponseStatusObservers();
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
                NotifyAuthObservers();
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

        public LogType LogType
        {
            get { return _logType; }
            set
            {
                SetProperty(ref _logType, value);
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

        public void Registration()
        {
            Task.Run(() => _mvvmNavigation.Navigate<RegistrationViewModel>());
        }

        public void Logout()
        {
            var refreshToken = _memoryCache.Get<string>("refreshToken");
            Task.Run(() => _httpClient.GetAsync($"Account/logout/{refreshToken}"));

            _memoryCache.Remove("refreshToken");
            _memoryCache.Remove("accessToken");
            _memoryCache.Remove("user");

            IsAuth = false;
            Email = string.Empty;
        }

        public void UploadCombatLogs()
        {
            Step = 0;
            Task.Run(() => _mvvmNavigation.Close(Parent));
        }

        public void GeneralAnalysis()
        {
            var dataForGeneralAnalysis = Tuple.Create(Combats, LogType);
            Task.Run(() => _mvvmNavigation.Navigate<GeneralAnalysisViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis));
        }

        public void DetailsSpecificalCombat()
        {
            Task.Run(() => _mvvmNavigation.Navigate<DetailsSpecificalCombatViewModel, CombatModel>(TargetCombat));
        }

        public void DamageDoneDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)Handler.Data;

            Task.Run(() => _mvvmNavigation.Close(Parent));
            Task.Run(() => _mvvmNavigation.Navigate<DamageDoneDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void HealDoneDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)Handler.Data;

            Task.Run(() => _mvvmNavigation.Close(Parent));
            Task.Run(() => _mvvmNavigation.Navigate<HealDoneDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void DamageTakenDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)Handler.Data;

            Task.Run(() => _mvvmNavigation.Close(Parent));
            Task.Run(() => _mvvmNavigation.Navigate<DamageTakenDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void ResourceDetails()
        {
            _combatInformtaion = (Tuple<int, CombatModel>)Handler.Data;

            Task.Run(() => _mvvmNavigation.Close(Parent));
            Task.Run(() => _mvvmNavigation.Navigate<ResourceRecoveryDetailsViewModel, Tuple<int, CombatModel>>(_combatInformtaion));
        }

        public void AddObserver(IResponseStatusObserver o)
        {
            _responseStatusObservers.Add(o);
        }

        public void RemoveObserver(IResponseStatusObserver o)
        {
            _responseStatusObservers.Remove(o);
        }

        public void NotifyResponseStatusObservers()
        {
            foreach (var item in _responseStatusObservers)
            {
                item.Update(ResponseStatus);
            }
        }

        public void AddObserver(IAuthObserver o)
        {
            _authObservers.Add(o);
        }

        public void RemoveObserver(IAuthObserver o)
        {
            _authObservers.Remove(o);
        }

        public void NotifyAuthObservers()
        {
            foreach (var item in _authObservers)
            {
                item.AuthUpdate(IsAuth);
            }
        }

        public void CheckAuth()
        {
            var user = _memoryCache.Get<UserModel>("user");
            if (user != null)
            {
                IsAuth = true;
                Email = user.Email;
            }
        }
    }
}