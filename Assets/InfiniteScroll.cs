using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class InfiniteScroll<T> : MonoBehaviour where T : Component
{
    private const float AdditionalDeltaCheck = 100f;

    [SerializeField] private GameObject _prefab;
    private Transform ContentParent => ScrollRect.content;

    private List<ScrolledItem<T>> _items;
    public ReadOnlyCollection<ScrolledItem<T>> Items => _items.AsReadOnly();

    public ScrollRect ScrollRect { get; private set; }

    [SerializeField] private Transform _leftBottomCorner;
    [SerializeField] private Transform _rightTopCorner;

    [SerializeField] private float _moveDownVelocity = 400;

    private Vector3 LeftBottomCorner => ContentParent.InverseTransformPoint(_leftBottomCorner.position);
    private Vector3 RightTopCorner => ContentParent.InverseTransformPoint(_rightTopCorner.position);

    private Vector2 _oldScrollPos; //В процентах

    private ISwapHelper _swapHelper;

    public void Init(int countPrefabs, Action<ScrolledItem<T>, int> firstInitializationAction, ISwapHelper swapHelper)
    {
        _swapHelper = swapHelper;
        ScrollRect = GetComponent<ScrollRect>();

        InstantiatePrefabs(countPrefabs);


        if (firstInitializationAction != null)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                firstInitializationAction.Invoke(_items[i], i);
            }
        }

        ScrollRect.onValueChanged.AddListener(DoScroll);
    }

    private void InstantiatePrefabs(int count)
    {
        _items = new();

        for (int i = 0; i < count; i++)
        {
            var itemObject = Instantiate(_prefab, ContentParent);
            var item = itemObject.GetComponent<T>();
            var scrolledItem = new ScrolledItem<T>();
            scrolledItem.Init(item);
            _items.Add(scrolledItem);
        }
    }

    private void DoScroll(Vector2 pos)
    {
        if (pos.y < _oldScrollPos.y)
        {
            while (ScrollMovingDown())
            {
            }
        }
        else
        {
            while (ScrollMovingUp())
            {
            }
        }

        _oldScrollPos = pos;
    }


    public bool ScrollMovingDown()
    {
        var lastView = _items[^1];
        var anchoredY = lastView.RectTransform.anchoredPosition.y;
        if (anchoredY >= LeftBottomCorner.y - (lastView.RectTransform.rect.height * 0.5f + AdditionalDeltaCheck))
        {
            return TrySwapDown();
        }

        return false;
    }

    public bool ScrollMovingUp()
    {
        var firstView = _items[0];
        var anchoredY = firstView.RectTransform.anchoredPosition.y;
        if (anchoredY <= RightTopCorner.y + firstView.RectTransform.rect.height * 0.5f + AdditionalDeltaCheck)
        {
            return TrySwapUp();
        }

        return false;
    }

    private void FixRectScrolling(Vector2 delta)
    {
        Type type = typeof(ScrollRect);
        FieldInfo myFieldInfo =
            type.GetField("m_PointerStartLocalCursor", BindingFlags.NonPublic | BindingFlags.Instance);
        var oldValue = (Vector2)myFieldInfo!.GetValue(ScrollRect);
        myFieldInfo.SetValue(ScrollRect, oldValue + delta);
    }


    private bool TrySwapDown()
    {
        var firstView = _items[0];
        var lastView = _items[^1];

        if (!_swapHelper.IsCanMoveToDown(firstView, lastView)) return false;

        //Swap
        firstView.RectTransform.transform.SetSiblingIndex(_items.Count - 1);
        _items.RemoveAt(0);
        _items.Add(firstView);

        var height = firstView.RectTransform.rect.height;
        FixRectScrolling(Vector2.up * height);
        ContentParent.transform.localPosition += Vector3.down * height;
        _swapHelper.MoveDownUpdateView(firstView, lastView);
        firstView.RectTransform.RecursiveForceRebuildLayoutImmediate();
        return true;
    }

    private bool TrySwapUp()
    {
        var firstView = _items[0];
        var lastView = _items[^1];

        if (!_swapHelper.IsCanMoveToUp(firstView, lastView)) return false;


        //Swap
        lastView.RectTransform.SetSiblingIndex(0);
        _items.RemoveAt(_items.Count - 1);
        _items.Insert(0, lastView);

        _swapHelper.MoveUpUpdateView(firstView, lastView);

        lastView.RectTransform.RecursiveForceRebuildLayoutImmediate();
        var height = lastView.RectTransform.rect.height;
        FixRectScrolling(Vector2.down * height);
        ContentParent.transform.localPosition -= Vector3.down * height;
        return true;
    }

    public void ScrollMoveToLastItem(bool isInstant = false)
    {
        if (isInstant)
        {
            ScrollRect.normalizedPosition = Vector2.zero;
        }
        else
        {
            // ScrollRect.velocity = Vector2.down * _velocityTest;
            StartCoroutine(SmoothScrollCoroutine());
        }
    }

    private IEnumerator SmoothScrollCoroutine()
    {
        while (true)
        {
            if (ScrollRect.IsDragging()) yield break;
            if (ScrollRect.normalizedPosition.y <= 0f)
            {
                ScrollRect.StopMovement();
                yield break;
            }

            ScrollRect.velocity = Vector2.up * _moveDownVelocity;
            yield return new WaitForEndOfFrame();
        }
    }

    public interface ISwapHelper
    {
        public bool IsCanMoveToUp(ScrolledItem<T> firstScrolledItem, ScrolledItem<T> lastScrolledItem);
        public bool IsCanMoveToDown(ScrolledItem<T> firstScrolledItem, ScrolledItem<T> lastScrolledItem);
        public void MoveUpUpdateView(ScrolledItem<T> firstScrolledItem, ScrolledItem<T> lastScrolledItem);
        public void MoveDownUpdateView(ScrolledItem<T> firstScrolledItem, ScrolledItem<T> lastScrolledItem);
    }
}