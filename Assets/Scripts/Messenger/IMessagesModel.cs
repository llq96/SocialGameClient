using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace VladB.SGC.Messenger
{
    public interface IMessagesModel
    {
        public UniTask Init();
        public List<MessageInfo> GetAllMessages();

        public event MessageDelegate OnNewMessageRecieved;

        public delegate void MessageDelegate(MessageInfo messageInfo);

        public void SendNewMessage(string message);
        public Sender GetCurrentSender();
        UniTask<MessageInfo> GetActualMessageInfo(int messageInfoIndex);
    }
}