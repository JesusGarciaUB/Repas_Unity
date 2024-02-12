using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UITable<T> : MonoBehaviour where T : UICell
{
    [SerializeField] protected ScrollRect _scrollRect;
    [SerializeField] private T _baseCell;

    protected virtual void Start()
    {
        if (_scrollRect == null)
        {
            Debug.LogWarning("Scroll rect not found in UITable named: " + name);
            return;
        }

        ReloadTable();
    }

    public void ReloadTable()
    {
        RectTransform parent = _scrollRect.content;

        int childCount = parent.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject gO = parent.GetChild(i).gameObject;
            if (gO != _baseCell.gameObject) Destroy(gO);
        }

        int cellsCount = TotalCellsCount;

        _baseCell.gameObject.SetActive(true);

        for (int i = 0;i < cellsCount; i++)
        {
            T cell = Instantiate(_baseCell, parent);
            cell.Index = i;

            SetupCell(cell);
        }

        _baseCell.gameObject.SetActive(false);
    }

    public abstract int TotalCellsCount { get; }
    public abstract void SetupCell(T cell);
}
