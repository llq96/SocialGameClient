using System;
using UnityEngine;

namespace VladB.Utility.InfiniteScroll
{
    public class ScrolledItem<T> where T : Component
    {
        public T Item { get; private set; }
        public RectTransform RectTransform { get; private set; }
        public Rect LastRect { get; private set; }

        public Action<ScrolledItem<T>, float> OnHeightChanged;

        public void Init(T item)
        {
            Item = item;
            RectTransform = item.transform.GetComponent<RectTransform>();
            LastRect = RectTransform.rect;
        }

        public void Update()
        {
            var heightDelta = RectTransform.rect.height - LastRect.height;

            if (Math.Abs(heightDelta) > 0.01f)
            {
                OnHeightChanged?.Invoke(this, heightDelta);
            }

            LastRect = RectTransform.rect;
        }
    }
}