using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragContainer : MonoBehaviour
{
    [NonSerialized] public RectTransform rect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
}
