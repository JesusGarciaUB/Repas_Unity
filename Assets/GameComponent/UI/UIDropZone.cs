using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDropZone : MonoBehaviour, IDropHandler
{
    public UnityEvent<UIDraggableObject, UIDropZone> OnDropDraggable;

    [SerializeField] private List<UITypes> _validUITypes = new();
    [SerializeField] private bool onEmptyAllValid;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped.TryGetComponent(out UIDraggableObject cell))
        {
            if (IsValidDraggable(cell))
            {
                cell._targetParent = this;
                OnDropDraggable.Invoke(cell, this);
            }
        }
    }

    private bool IsValidDraggable(UIDraggableObject cell)
    {
        if (_validUITypes.Count == 0)
        {
            if (onEmptyAllValid) return true;
            else return false;
        }

        foreach (UITypes types in cell.uiTypes)
        {
            if (_validUITypes.Contains(types)) return true;
        }

        return false;
    }
}
