using System.Collections.Generic;

public interface IMessagesModel
{
    public void Init();
    public List<MessageInfo> GetLastMessages();

    public event MessageDelegate OnNewMessageRecieved;

    public delegate void MessageDelegate(MessageInfo messageInfo);
}