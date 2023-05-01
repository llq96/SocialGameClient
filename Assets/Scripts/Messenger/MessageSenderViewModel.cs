namespace VladB.SGC.Messenger
{
    public class MessageSenderViewModel
    {
        private MessagesKeeper _messagesKeeper;

        public void Init(MessagesKeeper messagesKeeper)
        {
            _messagesKeeper = messagesKeeper;
        }

        public void SendNewMessage(string message)
        {
            _messagesKeeper.SendNewMessage(message);
        }
    }
}