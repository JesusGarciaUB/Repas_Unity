using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Debug Support")]
    [SerializeField] private Interactable _currentInteractable;

    [Header("Setup")]
    [SerializeField] private float _radiousRange = 0.5f;
    public float RadiousRange { get => _radiousRange; }

    private IEnumerator _checkBreakDistanceCorroutine;

    public void OnInteract(InputValue value)
    {
        if (value.isPressed) TryStartInteract();
        else TryEndInteract();
    }

    private void TryStartInteract()
    {
        Vector3 pos = transform.position;
        int inverseMask = ~(gameObject.layer);
        List<Collider> colliders = Physics.OverlapSphere(pos, _radiousRange, inverseMask, QueryTriggerInteraction.Collide).ToList();
        
        colliders.Sort((a, b) =>
        {
            float dA = Vector3.Distance(a.ClosestPoint(pos), pos), dB = Vector3.Distance(b.ClosestPoint(pos), pos);
            return dA.CompareTo(dB);
        });

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Interactable interactable))
            {
                _currentInteractable = interactable;
                _currentInteractable.StartInteract(gameObject);

                if (interactable.breakWithDistance)
                {
                    _checkBreakDistanceCorroutine = CheckBreakDistanceCorroutine(collider);
                    StartCoroutine(_checkBreakDistanceCorroutine);
                }
                
                return;
            }
        }
    }

    private void TryEndInteract()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.EndInteract(gameObject);
            _currentInteractable = null;
        }

        if (_checkBreakDistanceCorroutine != null)
        {
            StopCoroutine(_checkBreakDistanceCorroutine);
            _checkBreakDistanceCorroutine = null;
        }
    }

    private IEnumerator CheckBreakDistanceCorroutine(Collider target)
    {
        while(Vector3.Distance(target.ClosestPoint(transform.position), transform.position) <= _radiousRange)
        {
            yield return null;
        }

        TryEndInteract();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radiousRange);
    }
#endif
}
