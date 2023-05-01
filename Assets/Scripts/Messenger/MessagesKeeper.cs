using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class MessageInfo
{
    public int Index;
    public string Message;
}

[Serializable]
public class MessagesKeeper
{
    private SortedList<int, MessageInfo> _sortedList = new();

    public event OnNewMessageRecievedDelegate OnNewMessageRecieved;

    public delegate void OnNewMessageRecievedDelegate(MessageInfo messageInfo);

    private IMessagesModel _messagesModel;

    public void Init(IMessagesModel messagesModel)
    {
        _messagesModel = messagesModel;

        _sortedList.Clear();
        var lastMessages = _messagesModel.GetLastMessages();
        var keyValuePairs = lastMessages.Select(x => new KeyValuePair<int, MessageInfo>(x.Index, x));
        _sortedList.AddRange(keyValuePairs);

        _messagesModel.OnNewMessageRecieved += AddNewMessage;
    }

    private void AddNewMessage(MessageInfo messageInfo)
    {
        _sortedList.Add(messageInfo.Index, messageInfo);
        OnNewMessageRecieved?.Invoke(messageInfo);
    }

    public List<MessageInfo> GetLastMessages(int count = 20)
    {
        return _sortedList.Values.TakeLast(count).ToList();
    }


    public MessageInfo GetMessageBefore(MessageInfo messageInfo)
    {
        int index = _sortedList.IndexOfKey(messageInfo.Index);
        return _sortedList.TryGetValue(index - 1, out var message) ? message : null;
    }

    public MessageInfo GetMessageAfter(MessageInfo messageInfo)
    {
        int index = _sortedList.IndexOfKey(messageInfo.Index);
        return _sortedList.TryGetValue(index + 1, out var message) ? message : null;
    }
}