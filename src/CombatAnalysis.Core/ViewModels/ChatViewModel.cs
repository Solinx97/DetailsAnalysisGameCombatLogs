using CombatAnalysis.Core.Consts;
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

namespace CombatAnalysis.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel, IImprovedMvxViewModel
    {
        private const int ChatUpdateTimeIsMs = 500;

        private readonly IHttpClientHelper _httpClientHelper;

        private IImprovedMvxViewModel _basicTemplate;
        private IMemoryCache _memoryCache;
        private ObservableCollection<GroupChatMessageModel> _messages;
        private ObservableCollection<GroupChatModel> _groupChats;
        private bool _isMyMessage = true;
        private string _selectedChatName;
        private int _selectedGroupChatIndex = -1;
        private string _message;
        private string _myUsername;
        private Timer _timer;

        public ChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache)
        {
            _httpClientHelper = httpClientHelper;
            _memoryCache = memoryCache;

            Messages = new ObservableCollection<GroupChatMessageModel>();
            SendMessageCommand = new MvxAsyncCommand(SendAsync);
            CreateGroupChatCommand = new MvxCommand(CreateGroupChat);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;

            GetUser();
        } 

        public IMvxAsyncCommand SendMessageCommand { get; set; }

        public IMvxCommand CreateGroupChatCommand { get; set; }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }

            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public IViewModelConnect Handler { get; set; }

        public IMvxViewModel Parent { get; set; }

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

        public int SelectedGroupChatIndex
        {
            get { return _selectedGroupChatIndex; }
            set
            {
                SetProperty(ref _selectedGroupChatIndex, value);

                SelectedChatName = GroupChats[value].Name;
                _timer = new Timer(InitLoadGroupChatMessages, null, ChatUpdateTimeIsMs, Timeout.Infinite);
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

        public override void Prepare()
        {
            base.Prepare();

            Task.Run(GetGroupChats);
        }

        public async Task SendAsync()
        {
            var newGroupChatMessage = new GroupChatMessageModel
            {
                Message = Message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}"),
                Username = MyUsername,
                GroupChatId = GroupChats[SelectedGroupChatIndex].Id
            };

            Message = string.Empty;

            _httpClientHelper.BaseAddress = Port.ChatApi;
            await _httpClientHelper.PostAsync("GroupChatMessage", JsonContent.Create(newGroupChatMessage));
        }

        public void CreateGroupChat()
        {
            WindowManager.CreateGroupChat.Closed += CreateGroupChatClosed;
            WindowManager.CreateGroupChat.Show();
        }

        private void CreateGroupChatClosed(object sender, EventArgs e)
        {
            Task.Run(GetGroupChats);
        }

        private async Task GetGroupChats()
        {
            _httpClientHelper.BaseAddress = Port.ChatApi;

            var response = await _httpClientHelper.GetAsync("GroupChat");
            var groupChats = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatModel>>();
            GroupChats = new ObservableCollection<GroupChatModel>(groupChats);
        }

        private void InitLoadGroupChatMessages(object obj)
        {
            Task.Run(LoadGroupChatMessages);

            _timer.Change(ChatUpdateTimeIsMs, Timeout.Infinite);
        }

        private async Task LoadGroupChatMessages()
        {
            _httpClientHelper.BaseAddress = Port.ChatApi;

            var response = await _httpClientHelper.GetAsync("GroupChatMessage").ConfigureAwait(false);
            var groupChatMessages = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();
            var selectedGroupChatMessages = new List<GroupChatMessageModel>();
            foreach (var item in groupChatMessages)
            {
                if (item.GroupChatId == GroupChats[SelectedGroupChatIndex].Id)
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
