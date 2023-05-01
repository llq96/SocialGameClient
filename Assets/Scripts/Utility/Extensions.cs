using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static partial class Extensions
{
    public static void RecursiveForceRebuildLayoutImmediate(this Transform tr)
    {
        while (true)
        {
            if (tr == null) return;
            var rectTransform = tr.GetComponent<RectTransform>();
            if (rectTransform == null) return;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            tr = tr.parent;
        }
    }

    public static bool IsDragging(this ScrollRect scrollRect)
    {
        Type type = typeof(ScrollRect);
        FieldInfo myFieldInfo = type.GetField("m_Dragging", BindingFlags.NonPublic | BindingFlags.Instance);
        return (bool)myFieldInfo!.GetValue(scrollRect);
    }
}