using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static VladB.SGC.Messenger.IMessagesModel;
using Random = UnityEngine.Random;

namespace VladB.SGC.Messenger
{
    public class MockMessagesModel : MonoBehaviour, IMessagesModel
    {
        [SerializeField] private string _randomText;
        private List<MessageInfo> _messages;
        public event MessageDelegate OnNewMessageRecieved;

        private int LastMessageIndex => _messages.Count > 0 ? _messages.Last().Index : -1;

        [SerializeField] private int _countRandomMessages = 100001;

        private Sender _currentSender;

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
            PushNewMessage(message);
        }

        private void PushNewMessage(MessageInfo messageInfo)
        {
            _messages.Add(messageInfo);
            OnNewMessageRecieved?.Invoke(_messages.Last());
        }

        private void PushNewMessage(Sender sender, string message)
        {
            PushNewMessage(new MessageInfo(sender, LastMessageIndex + 1, message));
        }

        private MessageInfo GenerateRandomMessage(int index)
        {
            return new MessageInfo(GetRandomSender(), index, GetRandomText());
        }

        private Sender GetRandomSender()
        {
            return new Sender()
            {
                Name = new string(Guid.NewGuid().ToString().Take(8).ToArray())
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

        public void SendNewMessage(string message)
        {
            PushNewMessage(GetCurrentSender(), message);
        }

        public Sender GetCurrentSender()
        {
            return _currentSender ??= new Sender()
            {
                Name = "CurrentUser"
            };
        }
    }
}