using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;

public class DropPlace : MonoBehaviour
{
    [System.Flags]
    public enum CheckMode { CheckObject = 1, CheckObjectType = 2}

    [Header("Setup")]
    [SerializeField] private List<Grabable> _validGrabables = new();
    [SerializeField] private CheckMode _checkMode;
    [SerializeField] private List<ObjectType> _validObjectTypes = new();

    [Header("Events")]
    public UnityEvent<GameObject> OnObjectDropped;
    public UnityEvent<GameObject> OnObjectGrabbed;

    public bool IsValid(Grabable grabable)
    {
        if (_checkMode.HasFlag(CheckMode.CheckObject)) 
        {
            if (_validGrabables.Contains(grabable)) return true;
        }

        if (_checkMode.HasFlag(CheckMode.CheckObjectType))
        {
            foreach (ObjectType objectType in grabable.objectTypes)
            {
                if (_validObjectTypes.Contains(objectType)) return true;
            }
        }

        return false;
    }

    public void OnDrop(Grabable grabable)
    {
        OnObjectDropped.Invoke(grabable.gameObject);
        grabable.OnStartGrab.AddListener(OnGrab);
    }

    private void OnGrab(GameObject grabableObject, GameObject parent)
    {
        if(grabableObject.TryGetComponent(out Grabable grabable))
        {
            grabable.OnStartGrab.RemoveListener(OnGrab);
        }
        OnObjectGrabbed.Invoke(grabable.gameObject);
    }
}
