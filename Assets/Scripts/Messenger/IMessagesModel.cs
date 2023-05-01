using System.Collections.Generic;

namespace VladB.SGC.Messenger
{
    public interface IMessagesModel
    {
        public void Init();
        public List<MessageInfo> GetLastMessages();

        public event MessageDelegate OnNewMessageRecieved;

        public delegate void MessageDelegate(MessageInfo messageInfo);

        public void SendNewMessage(string message);
        public Sender GetCurrentSender();
    }
}