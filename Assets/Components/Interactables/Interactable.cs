using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] public bool breakWithDistance = true;

    [Header("Events")]
    public UnityEvent<GameObject> OnStartInteract;
    public UnityEvent<GameObject> OnEndInteract;

    public virtual void StartInteract(GameObject interactor)
    {
        OnStartInteract.Invoke(interactor);
    }

    public virtual void EndInteract(GameObject interactor)
    {
        OnEndInteract.Invoke(interactor);
    }
}
