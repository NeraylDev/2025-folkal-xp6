using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform _handPosition;
    [SerializeField] private float _handMovementDuration;
    [SerializeField] private Vector3 _offset;
    private Transform _mainCameraTransform;
    private Vector3 _defaultOffset;

    [Header("Throwing Settings")]
    [SerializeField] private Throwable _heldThrowable;
    [SerializeField] private float _minThrowingForce;
    [SerializeField] private float _maxThrowingForce;
    [SerializeField] private float _throwingLoadTime;
    [SerializeField] private float _throwingLoadOffsetZ;
    private float _throwingLoadTimer;
    private float _currentThrowingForce;
    private bool _loadingThrowing;

    private float _throwInputDelayTimer;
    private bool _canThrow;

    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance = 10f;
    [SerializeField] private LayerMask _interactableMask;

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

    private void Update()
    {
        if (!_canThrow)
        {
            _throwInputDelayTimer += Time.deltaTime;
            if (_throwInputDelayTimer >= 0.1f)
                _canThrow = true;
        }

        if (_loadingThrowing)
            UpdateThrowingForce();
    }

    private void FixedUpdate()
    {
        if (_heldThrowable != null)
        {
            UpdateThrowablePosition();
        }
    }

    #endregion


    private void UpdateThrowablePosition()
    {
        if (_mainCameraTransform != null)
        {
            _heldThrowable.transform.DOMove(transform.position + _offset, _handMovementDuration).SetEase(Ease.InBounce).SetUpdate(UpdateType.Fixed);
            _heldThrowable.transform.DOLookAt(transform.position + _mainCameraTransform.forward + _offset, _handMovementDuration).SetEase(Ease.InBounce).SetUpdate(UpdateType.Fixed);
        }
    }


    public void TryStartThrowing()
    {
        Debug.Log("Tenta arremessar");

        if (_heldThrowable == null || _loadingThrowing || !_canThrow)
            return;

        _currentThrowingForce = 0;
        _throwingLoadTimer = 0;
        _loadingThrowing = true;
    }

    private void UpdateThrowingForce()
    {
        _throwingLoadTimer += Time.deltaTime;
        _throwingLoadTimer = Mathf.Clamp01(_throwingLoadTimer);

        // Visual feedback of throwing force
        _currentThrowingForce = Mathf.Lerp(_minThrowingForce, _maxThrowingForce, _throwingLoadTimer);
        _offset = Vector3.Lerp(_defaultOffset, _defaultOffset + transform.forward * _throwingLoadOffsetZ, _throwingLoadTimer);
    }

    public void TryThrow()
    {
        if (!_loadingThrowing)
            return;

        // Apply throwing force on the object
        Throwable throwable = RemoveHeldThrowable();
        throwable.OnThrown();
        throwable.GetComponent<Rigidbody>().AddForce(throwable.transform.forward * _currentThrowingForce);

        _loadingThrowing = false;
    }



    public void SetHeldThrowable(Throwable throwable)
    {
        if (_heldThrowable != null) return;

        _offset = _defaultOffset;
        _heldThrowable = throwable;
        _heldThrowable.OnHeld();

        // Reseta delay do input de arremesso
        _throwInputDelayTimer = 0;
        _canThrow = false;
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
