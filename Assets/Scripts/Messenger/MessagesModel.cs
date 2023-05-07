using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using VladB.SGC;
using VladB.SGC.Messenger;
using Zenject;
using static VladB.SGC.Messenger.IMessagesModel;

namespace VladB.SGC.Messenger
{
    public class MessagesModel : IMessagesModel
    {
        private readonly WebRequester _webRequester;
        private readonly MessagesKeeper _keeper;

        private List<MessageInfo> _messages;
        public event MessageDelegate OnNewMessageRecieved;

        private int LastMessageIndex => _messages.Count > 0 ? _messages.Last().Index : -1;

        private Sender _currentSender;

        [Inject]
        public MessagesModel(WebRequester webRequester, MessagesKeeper keeper)
        {
            _webRequester = webRequester;
            _keeper = keeper;
        }

        public async UniTask Init()
        {
            var information = await _webRequester.GetMessagesInformation();
            _messages = new();
            for (int i = 0; i <= information.LastMessageIndex; i++)
            {
                _messages.Add(new MessageInfo(_keeper, i));
            }

            UpdateCircle().Forget();
        }

        public List<MessageInfo> GetAllMessages()
        {
            return _messages;
        }


        public void SendNewMessage(string message)
        {
            _webRequester.SendNewMessage(message).Forget();
        }

        public Sender GetCurrentSender()
        {
            return _currentSender ??= new Sender()
            {
                Name = "CurrentUser"
            };
        }

        public async UniTask<MessageInfo> GetActualMessageInfo(int messageInfoIndex)
        {
            return await _webRequester.DownloadMessage(messageInfoIndex);
        }

        private async UniTaskVoid UpdateCircle()
        {
            while (true)
            {
                await UniTask.Delay(1000 * 3, true);

                var information = await _webRequester.GetMessagesInformation();
                for (int i = _messages.Count; i <= information.LastMessageIndex; i++)
                {
                    var newMessageInfo = new MessageInfo(_keeper, i);
                    _messages.Add(newMessageInfo);
                    OnNewMessageRecieved?.Invoke(newMessageInfo);
                }
            }
        }
    }
}