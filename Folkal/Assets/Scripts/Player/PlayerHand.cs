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
    private Transform _cameraTransform;
    private Vector3 _offset;

    public Throwable GetHeldThrowable => _heldThrowable;
    public bool IsHoldingThrowable { get { return _heldThrowable != null; } }


    #region MonoBehaviour Methods

    private void FixedUpdate()
    {
        if (_heldThrowable != null)
        {
            UpdateThrowablePosition();
        }
    }

    #endregion

    public override void Initialize(PlayerManager playerManager, InputActionAsset actionAsset = null)
    {
        base.Initialize(playerManager, actionAsset);

        _cameraTransform = playerManager.GetCameraTransform;
    }

    public void SetOffsetZ(float z)
    {
        _offset = _cameraTransform.forward * z;
    }

    private void UpdateThrowablePosition()
    {
        if (_cameraTransform != null)
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
                _handPoint.position + _cameraTransform.forward + _offset,
                _handMovementDuration
            )
            .SetEase(Ease.InBounce)
            .SetUpdate(UpdateType.Fixed);
        }
    }

    public void PickUpThrowable(Throwable throwable)
    {
        if (IsHoldingThrowable)
            return;

        SetHeldThrowable(throwable);

        _playerManager.GetPlayerThrowing.ResetInputDelayTimer();
        _playerManager.GetEvents.RaisePickUpThrowable(_playerManager);
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
