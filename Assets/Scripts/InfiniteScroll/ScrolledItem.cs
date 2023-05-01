using UnityEngine;

namespace VladB.Utility.InfiniteScroll
{
    public class ScrolledItem<T> where T : Component
    {
        public T Item { get; private set; }
        public RectTransform RectTransform { get; private set; }

        public void Init(T item)
        {
            Item = item;
            RectTransform = item.transform.GetComponent<RectTransform>();
        }
    }
}