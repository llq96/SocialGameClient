using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;


namespace VladB.SGC.Messenger
{
    public class MessageInfo
    {
        private MessagesKeeper _keeper;
        [JsonProperty] public int Index { get; }
        [JsonProperty] public Sender Sender { get; private set; }
        [JsonProperty] public string Message { get; private set; }
        public bool IsLoaded { get; private set; }

        public Action OnMessageInfoUpdated;

        public MessageInfo()
        {
        }

        public MessageInfo(MessagesKeeper keeper, int index)
        {
            _keeper = keeper;
            Index = index;
        }

        public MessageInfo(MessagesKeeper keeper, Sender sender, int index, string message)
        {
            Sender = sender;
            Index = index;
            Message = message;
        }

        public void UpdateMessageInfo(MessageInfo copyFrom)
        {
            Sender = copyFrom.Sender;
            Message = copyFrom.Message;

            IsLoaded = true; //TODO

            Debug.Log("Loaded");
            OnMessageInfoUpdated?.Invoke();
        }

        public void TryUpdateMessageInfo()
        {
            _keeper.UpdateMessageInfo(this);
        }
    }

    public class Sender
    {
        public string Name;
    }


    public class MessagesKeeper
    {
        public async void UpdateMessageInfo(MessageInfo messageInfo)
        {
            if (!messageInfo.IsLoaded)
            {
                var downloadedMessageInfo = await _messagesModel.GetActualMessageInfo(messageInfo.Index);
                messageInfo.UpdateMessageInfo(downloadedMessageInfo);
            }
        }

        private SortedList<int, MessageInfo> _sortedList = new();

        public event OnNewMessageRecievedDelegate OnNewMessageRecieved;

        public delegate void OnNewMessageRecievedDelegate(MessageInfo messageInfo);

        private IMessagesModel _messagesModel;

        public void Init(IMessagesModel messagesModel)
        {
            _messagesModel = messagesModel;

            _sortedList.Clear();
            var lastMessages = _messagesModel.GetAllMessages();
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

        public void SendNewMessage(string message)
        {
            _messagesModel.SendNewMessage(message);
        }
    }
}