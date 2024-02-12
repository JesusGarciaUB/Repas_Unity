using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(CanvasGroup))]

public class UIDraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rect;
    public UIDropZone _targetParent;
    private Transform _ogParent;
    private CanvasGroup _ignoreRaycastComponent;

    [SerializeField] public List<UITypes> uiTypes = new();

    public UnityEvent<UIDraggableObject, UIDropZone> OnDragStart;
    public UnityEvent<UIDraggableObject> OnFailedDrop;
    public UnityEvent<UIDraggableObject> OnSuccessDrop;

    private void Start()
    {
        _ignoreRaycastComponent = GetComponent<CanvasGroup>();
        _rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ignoreRaycastComponent.blocksRaycasts = false;
        _targetParent = _rect.parent.GetComponent<UIDropZone>();
        _ogParent = _targetParent.transform;
        _rect.SetParent(GetComponentInParent<DragContainer>().rect);
        OnDragStart.Invoke(this, _targetParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null || eventData.pointerEnter.transform as RectTransform == null) return;

        RectTransform plane = eventData.pointerEnter.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(plane, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
        {
            _rect.position = globalMousePos;
            _rect.rotation = plane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _ignoreRaycastComponent.blocksRaycasts = true;
        _rect.SetParent(_targetParent.transform);
        if (_targetParent == _ogParent) OnFailedDrop.Invoke(this);
        else OnSuccessDrop.Invoke(this);
    }
}
