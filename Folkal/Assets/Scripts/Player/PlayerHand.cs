using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : PlayerSubsystem
{
    [SerializeField] private Transform _handPosition;
    [SerializeField] private Throwable _heldThrowable;
    [SerializeField] private float _handMovementDuration;
    [SerializeField] private Vector3 _offset;
    private Vector3 _offsetModifier;
    private Transform _mainCameraTransform;
    private Vector3 _defaultOffset;

    public static PlayerHand instance;

    public Throwable GetHeldThrowable => _heldThrowable;
    public bool IsHoldingThrowable { get { return _heldThrowable != null; } }


    #region MonoBehaviour Methods

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        _defaultOffset = _offset;
    }

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

    public void SetOffsetModifier(Vector3 offset)
        => _offsetModifier = offset;

    private void UpdateThrowablePosition()
    {
        if (_mainCameraTransform != null)
        {
            _heldThrowable.transform.DOMove(transform.position + _offset + _offsetModifier, _handMovementDuration).SetEase(Ease.InBounce).SetUpdate(UpdateType.Fixed);
            _heldThrowable.transform.DOLookAt(transform.position + _mainCameraTransform.forward + _offset + _offsetModifier, _handMovementDuration).SetEase(Ease.InBounce).SetUpdate(UpdateType.Fixed);
        }
    }

    public void SetHeldThrowable(Throwable throwable)
    {
        if (_heldThrowable != null) return;

        _offset = _defaultOffset;
        _heldThrowable = throwable;
        _heldThrowable.OnHeld();

        _playerManager.GetPlayerThrowing.ResetInputDelayTimer();
    }

    public Throwable RemoveHeldThrowable()
    {
        if (_heldThrowable == null) return null;

        Throwable temp = _heldThrowable;
        _heldThrowable.EnableRigidbody();
        _heldThrowable = null;

        return temp;
    }

}
