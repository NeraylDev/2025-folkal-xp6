using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : PlayerSubsystem
{
    [SerializeField] private Transform _handPoint;
    [SerializeField] private Throwable _heldThrowable;
    [SerializeField] private float _handMovementDuration;
    private Vector3 _offset;
    private Transform _mainCameraTransform;

    public Throwable GetHeldThrowable => _heldThrowable;
    public bool IsHoldingThrowable { get { return _heldThrowable != null; } }


    #region MonoBehaviour Methods

    private void Start()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (_heldThrowable != null)
        {
            UpdateThrowablePosition();
        }
    }

    #endregion

    public void SetZOffset(float z)
    {
        _offset = _mainCameraTransform.forward * z;
    }

    private void UpdateThrowablePosition()
    {
        if (_mainCameraTransform != null)
        {
            _heldThrowable.transform.DOMove
            (
                _handPoint.position + _offset,
                _handMovementDuration
            )
            .SetEase(Ease.InBounce)
            .SetUpdate(UpdateType.Fixed);

            _heldThrowable.transform.DOLookAt
            (
                _handPoint.position + _mainCameraTransform.forward + _offset,
                _handMovementDuration
            )
            .SetEase(Ease.InBounce)
            .SetUpdate(UpdateType.Fixed);
        }
    }

    public void SetHeldThrowable(Throwable throwable)
    {
        _heldThrowable = throwable;
        _heldThrowable.OnHeld();
    }

    public Throwable RemoveHeldThrowable()
    {
        Throwable temp = _heldThrowable;
        _heldThrowable.EnableRigidbody();
        _heldThrowable = null;

        return temp;
    }

}
