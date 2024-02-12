using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Translator : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private float _currentTime = 0f;

    //public enum AnimationMode { Lineal, Curve }
    //[SerializeField] AnimationMode _animationMode = AnimationMode.Lineal;
    [SerializeField] AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);


    [Header("Translations")]
    [SerializeField] private Vector3 _displacement = Vector3.zero;
    private Vector3 _originPosition;
    [SerializeField] private Vector3 _rotation = Vector3.zero;
    private Quaternion _originRotation;

    private IEnumerator _currentAnimation;

    [Header("Animation Triggers")]
    public UnityEvent OnOriginReach;
    public UnityEvent OnTargetReach;
    public UnityEvent<float> OnChange;

    

    private void Awake()
    {
        _originPosition = transform.localPosition;
        _originRotation = transform.localRotation;
    }

    public void ToOrigin()
    {
        ChangeAnimation(ToOriginAnimation());
    }

    public void ToTarget()
    {
        ChangeAnimation(ToTargetAnimation());
    }

    private void ChangeAnimation(IEnumerator newAnimation)
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
        }

        _currentAnimation = newAnimation;
        StartCoroutine(_currentAnimation);
    }

    private IEnumerator ToOriginAnimation()
    {
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;

            SetPositionForCurrentTime();

            yield return new WaitForEndOfFrame();
        }

        _currentTime = 0f;
        SetPositionForCurrentTime();

        _currentAnimation = null;
        OnOriginReach.Invoke();
    }

    private IEnumerator ToTargetAnimation()
    {
        while (_currentTime < _animationDuration)
        {
            _currentTime += Time.deltaTime;

            SetPositionForCurrentTime();

            yield return new WaitForEndOfFrame();
        }

        _currentTime = _animationDuration;
        SetPositionForCurrentTime();

        _currentAnimation = null;
        OnTargetReach.Invoke();
    }

    private void SetPositionForCurrentTime()
    {
        float interpolatedValue = _currentTime / _animationDuration;

        //switch (_animationMode)
        //{
        //    case AnimationMode.Lineal:
        //        break;

        //    case AnimationMode.Curve:
        //        interpolatedValue = Mathf.Sin(interpolatedValue);
        //        break;
        //}

        interpolatedValue = _curve.Evaluate(interpolatedValue);

        transform.localPosition = _originPosition + (_displacement * interpolatedValue);
        Vector3 newRotation = _rotation * interpolatedValue;
        transform.localRotation = _originRotation * Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z);

        OnChange.Invoke(interpolatedValue);
    }
}
