using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    [Header("Objects Setup")]
    [SerializeField, Min(1)] private uint _requiredAmountObjects = 1;
    [SerializeField] private List<GameObject> _objectsInside = new();

    [Header("Events")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeativated;

    private void OnTriggerEnter(Collider other)
    {
        _objectsInside.Add(other.gameObject);

        if (_objectsInside.Count == _requiredAmountObjects)
        {
            OnActivated.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _objectsInside.Remove(other.gameObject);

        if (_objectsInside.Count == _requiredAmountObjects - 1)
        {
            OnDeativated.Invoke();
        }
    }
}
