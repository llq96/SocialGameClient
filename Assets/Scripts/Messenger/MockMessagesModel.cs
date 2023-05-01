using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IMessagesModel;

public class MockMessagesModel : MonoBehaviour, IMessagesModel
{
    [SerializeField] private string _randomText;
    private List<MessageInfo> _messages;
    public event MessageDelegate OnNewMessageRecieved;
    private int LastMessageIndex => _messages.Count > 0 ? _messages.Last().Index : -1;

    [SerializeField] private int _countRandomMessages = 100001;

    public void Init()
    {
        _messages = new();
        for (int i = 0; i < _countRandomMessages; i++)
        {
            AddRandomMessage();
        }
    }

    [ContextMenu(nameof(AddRandomMessage))]
    private void AddRandomMessage()
    {
        var message = GenerateRandomMessage(LastMessageIndex + 1);
        _messages.Add(message);
        OnNewMessageRecieved?.Invoke(_messages.Last());
    }


    private MessageInfo GenerateRandomMessage(int index)
    {
        return new MessageInfo()
        {
            Index = index,
            Message = GetRandomText()
        };
    }


    public List<MessageInfo> GetLastMessages()
    {
        return _messages;
    }

    private string GetRandomText()
    {
        return _randomText.Substring(Random.Range(0, _randomText.Length - 300), Random.Range(0, 300));
    }
}