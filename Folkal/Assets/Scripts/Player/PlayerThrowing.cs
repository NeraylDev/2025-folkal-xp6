using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowing : PlayerSubsystem
{
    [SerializeField] private float _minThrowingForce;
    [SerializeField] private float _maxThrowingForce;
    [SerializeField] private float _throwingLoadDuration;
    [SerializeField] private float _throwingLoadOffsetZ;
    private float _currentThrowingForce;
    private float _throwInputDelayTimer;
    private float _throwingLoadTimer;
    private bool _isLoadingThrow;
    private bool _hasThrewObject;
    private bool _canThrow;

    private Tweener _throwingLoadTween;

    public bool IsLoadingThrow => _isLoadingThrow;
    public bool HasThrewObject => _hasThrewObject;


    #region MonoBehaviour Methods

    private void Update()
    {
        if (!_canThrow)
        {
            _throwInputDelayTimer += Time.deltaTime;
            if (_throwInputDelayTimer >= 0.1f)
                _canThrow = true;
        }

        if (_isLoadingThrow)
            UpdateThrowingForce();
    }

    #endregion

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        actionAsset.FindAction("Action").started += (InputAction.CallbackContext context) => TryStartThrowing();
        actionAsset.FindAction("Action").canceled += (InputAction.CallbackContext context) => TryThrow();
    }

    public void ResetInputDelayTimer()
    {
        _throwInputDelayTimer = 0;
        _hasThrewObject = false;
        _canThrow = false;
    }

    private void TryStartThrowing()
    {
        if (!_canThrow || _isLoadingThrow
            || !_playerManager.GetPlayerHand.IsHoldingThrowable
            || !_playerManager.GetPlayerMovement.CanMove)
            return;

        _currentThrowingForce = 0;
        _throwingLoadTimer = 0;
        _isLoadingThrow = true;
    }

    private void UpdateThrowingForce()
    {
        if (_throwingLoadTween == null)
        {
            Tweener throwingLoadTween = DOTween.To
            (
                () => _throwingLoadTimer,
                x => _throwingLoadTimer = x,
                1, _throwingLoadDuration
            )
            .SetEase(Ease.OutQuad);

            _throwingLoadTween = throwingLoadTween;
        }

        _currentThrowingForce = Mathf.Lerp(_minThrowingForce, _maxThrowingForce, _throwingLoadTimer);
        
        PlayerHand playerHand = _playerManager.GetPlayerHand;
        float throwableOffset = Mathf.Lerp(0, _throwingLoadOffsetZ, _throwingLoadTimer);
        
        _playerManager.GetPlayerHand.SetZOffset(throwableOffset);
    }

    private void TryThrow()
    {
        if (!_isLoadingThrow || !_playerManager.GetPlayerHand.IsHoldingThrowable)
            return;

        _throwingLoadTween.Kill();
        _throwingLoadTween = null;
        _playerManager.GetPlayerHand.SetZOffset(0);

        Throwable throwable = _playerManager.DropThrowable();
        ApplyForce(throwable);

        _isLoadingThrow = false;
        _hasThrewObject = true;
    }

    private void ApplyForce(Throwable throwable)
    {
        if (throwable == null)
            return;

        throwable.OnThrown();
        if (throwable.TryGetComponent(out Rigidbody rigidbody))
        {
            Vector3 force = _playerManager.GetPlayerCamera.transform.forward * _currentThrowingForce;
            rigidbody.AddForce(force);
        }
    }
}
