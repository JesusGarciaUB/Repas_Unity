using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Grabable : MonoBehaviour
{
    public enum GrabMode { FollowParentTeleport, FollowParentSmooth, SetOnParent, SetOnParentZeroPosition}

    [Header("Setup")]
    [SerializeField] private GrabMode _grabMode = GrabMode.FollowParentTeleport;
    [SerializeField, Min(0)] private float _smoothTime = 0.25f;
    [SerializeField] public List<ObjectType> objectTypes = new();

    [Header("Events")]
    public UnityEvent<GameObject, GameObject> OnStartGrab;
    public UnityEvent<GameObject, GameObject> OnEndGrab;

    private IEnumerator _followCoroutine;
    private Vector3 _currentVelocity = Vector3.zero;

    private bool _isGrabed = false;

    public void GrabSwitch(GameObject parent)
    {
        if (_isGrabed) EndGrab(parent);
        else StartGrab(parent);
    }

    public void StartGrabHold(GameObject parent)
    {
        StartGrab(parent);
    }

    private void StartGrab(GameObject parent)
    {
        _isGrabed = true;

        if (_followCoroutine != null )
        {
            StopCoroutine( _followCoroutine );
            _followCoroutine = null;
        }

        switch (_grabMode)
        {
            case GrabMode.FollowParentTeleport:
            case GrabMode.FollowParentSmooth:
                transform.parent = null;
                _followCoroutine = FollowCoroutine(parent);
                StartCoroutine( _followCoroutine );
                break;
            case GrabMode.SetOnParent:
                transform.parent = parent.transform;
                break;
            case GrabMode.SetOnParentZeroPosition:
                transform.parent = parent.transform;
                transform.localPosition = Vector3.zero;
                break;
        }

        OnStartGrab.Invoke(gameObject, parent);
    }

    public void EndGrab(GameObject parent)
    {
        _isGrabed = false;

        if (_followCoroutine != null )
        {
            StopCoroutine( _followCoroutine );
            _followCoroutine = null;
        }

        transform.parent = null;

        TryDrop(parent);

        OnEndGrab.Invoke(gameObject, parent);
    }

    private IEnumerator FollowCoroutine(GameObject parent)
    {
        while(true)
        {
            switch (_grabMode)
            {
                case GrabMode.FollowParentTeleport:
                    transform.position = parent.transform.position;
                    break;
                case GrabMode.FollowParentSmooth:
                    transform.position = Vector3.SmoothDamp(transform.position, parent.transform.position, ref _currentVelocity, _smoothTime);
                    break;
                case GrabMode.SetOnParent:
                case GrabMode.SetOnParentZeroPosition:
                    break;
            }

            yield return null;
        }
    }

    private void TryDrop(GameObject parent)
    {
        Vector3 pos = parent.transform.position;
        int inverseMask = ~(parent.layer);

        Interactor interactorParent = parent.GetComponent<Interactor>();

        if (interactorParent == null)
        {
            return;
        }

        List<Collider> colliders = Physics.OverlapSphere(pos, interactorParent.RadiousRange, inverseMask, QueryTriggerInteraction.Collide).ToList();

        colliders.Sort((a, b) =>
        {
            float dA = Vector3.Distance(a.ClosestPoint(pos), pos), dB = Vector3.Distance(b.ClosestPoint(pos), pos);
            return dA.CompareTo(dB);
        });

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out DropPlace dropPlace))
            {
                dropPlace.OnDrop(this);
            }
        }
    }
}
