using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CombatAnalysis.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel, IImprovedMvxViewModel
    {
        private const int GroupChatMessagesUpdateTimeIsMs = 500;

        private readonly IHttpClientHelper _httpClientHelper;

        private IImprovedMvxViewModel _basicTemplate;
        private IMemoryCache _memoryCache;
        private ObservableCollection<GroupChatMessageModel> _messages;
        private ObservableCollection<GroupChatModel> _groupChats;
        private ObservableCollection<PersonalChatModel> _personalChats;
        private bool _isMyMessage = true;
        private string _selectedChatName;
        private GroupChatModel _selectedGroupChat;
        private int _selectedGroupChatIndex = -1;
        private string _message;
        private Visibility _showGroupChatMenu = Visibility.Collapsed;
        private ResponseStatus _groupChatLoadingResponse = ResponseStatus.None;
        private ResponseStatus _personalChatLoadingResponse = ResponseStatus.Pending;
        private string _myUsername = string.Empty;
        private Timer _groupChatMessagesUpdateTimer;

        public ChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache)
        {
            _httpClientHelper = httpClientHelper;
            _memoryCache = memoryCache;

            Messages = new ObservableCollection<GroupChatMessageModel>();
            SendMessageCommand = new MvxAsyncCommand(SendAsync);
            CreateGroupChatCommand = new MvxCommand(CreateGroupChat);
            RefreshGroupChatCommand = new MvxAsyncCommand(LoadGroupChats);
            ShowGroupChatMenuComand = new MvxCommand(ShowGCMenu);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;

            GetUser();
        } 

        public IMvxAsyncCommand SendMessageCommand { get; set; }

        public IMvxCommand CreateGroupChatCommand { get; set; }

        public IMvxAsyncCommand RefreshGroupChatCommand { get; set; }

        public IMvxCommand ShowGroupChatMenuComand { get; set; }

        public IViewModelConnect Handler { get; set; }

        public IMvxViewModel Parent { get; set; }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }

            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<GroupChatMessageModel> Messages
        {
            get { return _messages; }
            set
            {
                SetProperty(ref _messages, value);
            }
        }

        public ObservableCollection<GroupChatModel> GroupChats
        {
            get { return _groupChats; }
            set
            {
                SetProperty(ref _groupChats, value);
            }
        }

        public ObservableCollection<PersonalChatModel> PersonalChats
        {
            get { return _personalChats; }
            set
            {
                SetProperty(ref _personalChats, value);
            }
        }

        public int SelectedGroupChatIndex
        {
            get { return _selectedGroupChatIndex; }
            set
            {
                SetProperty(ref _selectedGroupChatIndex, value);
            }
        }

        public GroupChatModel SelectedGroupChat
        {
            get { return _selectedGroupChat; }
            set
            {
                SetProperty(ref _selectedGroupChat, value);

                if (value != null)
                {
                    SelectedChatName = value.Name;
                    _groupChatMessagesUpdateTimer = new Timer(InitLoadGroupChatMessages, null, GroupChatMessagesUpdateTimeIsMs, Timeout.Infinite);
                }
            }
        }

        public string SelectedChatName
        {
            get { return _selectedChatName; }
            set
            {
                SetProperty(ref _selectedChatName, value);
            }
        }

        public bool IsMyMessage
        {
            get { return _isMyMessage; }
            set
            {
                SetProperty(ref _isMyMessage, value);
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                SetProperty(ref _message, value);
            }
        }

        public string MyUsername
        {
            get { return _myUsername; }
            set
            {
                SetProperty(ref _myUsername, value);
            }
        }
        
        public Visibility ShowGroupChatMenu
        {
            get { return _showGroupChatMenu; }
            set
            {
                SetProperty(ref _showGroupChatMenu, value);
            }
        }

        public ResponseStatus GroupChatLoadingResponse
        {
            get { return _groupChatLoadingResponse; }
            set
            {
                SetProperty(ref _groupChatLoadingResponse, value);
            }
        }

        public ResponseStatus PersonalChatLoadingResponse
        {
            get { return _personalChatLoadingResponse; }
            set
            {
                SetProperty(ref _personalChatLoadingResponse, value);
            }
        }

        public override void Prepare()
        {
            base.Prepare();

            Task.Run(LoadGroupChats);
        }

        public async Task SendAsync()
        {
            var newGroupChatMessage = new GroupChatMessageModel
            {
                Message = Message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}"),
                Username = MyUsername,
                GroupChatId = SelectedGroupChat.Id
            };

            Message = string.Empty;

            _httpClientHelper.BaseAddress = Port.ChatApi;
            await _httpClientHelper.PostAsync("GroupChatMessage", JsonContent.Create(newGroupChatMessage));

            SelectedGroupChat.LastMessage = newGroupChatMessage.Message;

            await _httpClientHelper.PutAsync("GroupChat", JsonContent.Create(SelectedGroupChat));
        }

        public void CreateGroupChat()
        {
            WindowManager.CreateGroupChat.Show();
        }

        public void ShowGCMenu()
        {
            if (ShowGroupChatMenu == Visibility.Visible)
            {
                ShowGroupChatMenu = Visibility.Collapsed;
            }
            else
            {
                ShowGroupChatMenu = Visibility.Visible;
            }
        }

        private void InitLoadGroupChatMessages(object obj)
        {
            Task.Run(LoadGroupChatMessages);

            if (SelectedGroupChat != null)
            {
                _groupChatMessagesUpdateTimer.Change(GroupChatMessagesUpdateTimeIsMs, Timeout.Infinite);
            }
        }

        private async Task LoadGroupChats()
        {
            GroupChatLoadingResponse = ResponseStatus.Pending;

            _httpClientHelper.BaseAddress = Port.ChatApi;

            var response = await _httpClientHelper.GetAsync("GroupChat");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var groupChats = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatModel>>();

                GroupChats = new ObservableCollection<GroupChatModel>(groupChats);

                GroupChatLoadingResponse = ResponseStatus.Successful;
            }
            else
            {
                GroupChatLoadingResponse = ResponseStatus.Failed;
            }
        }

        private async Task LoadGroupChatMessages()
        {
            _httpClientHelper.BaseAddress = Port.ChatApi;

            var response = await _httpClientHelper.GetAsync("GroupChatMessage").ConfigureAwait(false);
            var groupChatMessages = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();
            var selectedGroupChatMessages = new List<GroupChatMessageModel>();
            foreach (var item in groupChatMessages)
            {
                if (SelectedGroupChat != null && item.GroupChatId == SelectedGroupChat.Id)
                {
                    selectedGroupChatMessages.Add(item);
                }
            }

            Messages = new ObservableCollection<GroupChatMessageModel>(selectedGroupChatMessages);
        }

        private void GetUser()
        {
            var user = _memoryCache.Get<AppUserModel>("user");
            if (user != null)
            {
                MyUsername = user.Email;
            }
        }
    }
}
