using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowing : PlayerSubsystem
{
    [SerializeField] private float _minThrowingForce;
    [SerializeField] private float _maxThrowingForce;
    [SerializeField] private float _throwChargeDuration;
    [SerializeField] private float _throwChargeOffsetZ;
    private float _currentThrowingForce;
    private float _throwInputDelayTimer;
    private bool _isChargingThrow;
    private bool _hasThrewObject;
    private bool _canThrow;

    public float GetThrowChargeDuration => _throwChargeDuration;
    public float GetThrowChargeOffsetZ => _throwChargeOffsetZ;

    public bool IsChargingThrow => _isChargingThrow;
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
    }

    #endregion

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        actionAsset.FindAction("Action").started += (InputAction.CallbackContext context) => TryStartThrowing();
        actionAsset.FindAction("Action").canceled += (InputAction.CallbackContext context) => TryThrow();
    }

    private void TryStartThrowing()
    {
        if (!_canThrow || _isChargingThrow
            || !_playerManager.GetPlayerHand.IsHoldingThrowable
            || !_playerManager.GetPlayerMovement.CanMove)
            return;

        _currentThrowingForce = 0;
        _isChargingThrow = true;
    }

    private void TryThrow()
    {
        if (!_isChargingThrow || !_playerManager.GetPlayerHand.IsHoldingThrowable)
            return;

        _isChargingThrow = false;
        _hasThrewObject = true;
    }

    public void SetThrowingForce(float normalizedValue)
    {
        _currentThrowingForce = Mathf.Lerp(_minThrowingForce, _maxThrowingForce, normalizedValue);
    }

    public void ResetInputDelayTimer()
    {
        _throwInputDelayTimer = 0;
        _hasThrewObject = false;
        _canThrow = false;
    }

    public void ApplyForce(Throwable throwable)
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
