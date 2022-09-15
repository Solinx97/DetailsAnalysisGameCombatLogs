using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace CombatAnalysis.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel, IImprovedMvxViewModel
    {
        private readonly IHttpClientHelper _httpClientHelper;

        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<MessageDataModel> _messages;
        private bool _isMyMessage = true;
        private string _message;

        public ChatViewModel(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;

            Messages = new ObservableCollection<MessageDataModel>();
            SendMessageCommand = new MvxCommand(Send);
            CreateGroupChatCommand = new MvxCommand(CreateGroupChat);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;
        } 

        public IMvxCommand SendMessageCommand { get; set; }

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

        public ObservableCollection<MessageDataModel> Messages
        {
            get { return _messages; }
            set
            {
                SetProperty(ref _messages, value);
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

        public void Send()
        {
            var newMessage = new MessageDataModel
            {
                IsMyMessage = true,
                DayTimeType = (int)Enums.WhenType.Today,
                Message = Message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}"),
                Username = "Dima"
            };

            Messages.Add(newMessage);
            Message = string.Empty;

            _httpClientHelper.BaseAddress = Port.ChatApi;
            Task.Run(async () => await _httpClientHelper.PostAsync("MessageData", JsonContent.Create(newMessage)));
        }

        public void CreateGroupChat()
        {
            WindowManager.CreateGroupChat.Show();
        }
    }
}
