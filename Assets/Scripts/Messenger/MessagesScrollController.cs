using System.Linq;
using UnityEngine;
using VladB.Utility.InfiniteScroll;

namespace VladB.SGC.Messenger
{
    public class MessagesScrollController : MonoBehaviour, InfiniteScroll<MessageView>.ISwapHelper
    {
        [SerializeField] private MessagesScrollView _messagesScrollView;
        private MessagesKeeper _messagesKeeper;

        [SerializeField] private int _prefabsCount = 10;

        public void Init(MessagesKeeper messagesKeeper)
        {
            _messagesKeeper = messagesKeeper;
            _messagesKeeper.OnNewMessageRecieved += TryAddNewMessage;
            var lastMessages = _messagesKeeper.GetLastMessages(_prefabsCount);
            int countCanSpawn = Mathf.Min(lastMessages.Count, _prefabsCount);
            int startIndex = _prefabsCount - countCanSpawn;

            _messagesScrollView.Init(_prefabsCount, FirstInit, this);

            _messagesScrollView.ScrollMoveToLastItem(true);

            void FirstInit(ScrolledItem<MessageView> scrolledItem, int index)
            {
                if (index >= startIndex)
                {
                    var message = lastMessages[lastMessages.Count - countCanSpawn + index - startIndex];
                    scrolledItem.Item.SetMessage(message);
                }
                else
                {
                    scrolledItem.Item.SetNullText();
                }
            }
        }

        private void TryAddNewMessage(MessageInfo messageinfo)
        {
            var lastScrolledItem = _messagesScrollView.Items.Last();
            if (lastScrolledItem == null) return;
            var item = lastScrolledItem.Item;
            if (item == null) return;
            if (_messagesKeeper.GetMessageAfter(item.MessageInfo) == messageinfo)
            {
                if (_messagesScrollView.ScrollMovingDown())
                {
                    _messagesScrollView.ScrollMoveToLastItem();
                    // MoveScrollToLastMessage();
                }
            }
        }


        public bool IsCanMoveToUp(ScrolledItem<MessageView> firstScrolledItem,
            ScrolledItem<MessageView> lastScrolledItem)
        {
            var message = _messagesKeeper.GetMessageBefore(firstScrolledItem.Item.MessageInfo);
            if (message == null)
            {
                // Debug.Log("Not Have Old Message");
                return false;
            }

            return true;
        }

        public bool IsCanMoveToDown(ScrolledItem<MessageView> firstScrolledItem,
            ScrolledItem<MessageView> lastScrolledItem)
        {
            var message = _messagesKeeper.GetMessageAfter(lastScrolledItem.Item.MessageInfo);
            if (message == null)
            {
                // Debug.Log("Not Have New Message");
                return false;
            }

            return true;
        }

        public void MoveUpUpdateView(ScrolledItem<MessageView> firstScrolledItem,
            ScrolledItem<MessageView> lastScrolledItem)
        {
            // Debug.Log(nameof(MoveUpUpdateView));
            var message = _messagesKeeper.GetMessageBefore(firstScrolledItem.Item.MessageInfo);
            lastScrolledItem.Item.SetMessage(message);
        }

        public void MoveDownUpdateView(ScrolledItem<MessageView> firstScrolledItem,
            ScrolledItem<MessageView> lastScrolledItem)
        {
            // Debug.Log(nameof(MoveDownUpdateView));
            var message = _messagesKeeper.GetMessageAfter(lastScrolledItem.Item.MessageInfo);
            firstScrolledItem.Item.SetMessage(message);
        }
    }
}