using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowing : PlayerSubsystem
{
    [SerializeField] private float _minThrowingForce;
    [SerializeField] private float _maxThrowingForce;
    [SerializeField] private float _throwingLoadDuration;
    [SerializeField] private float _throwingLoadOffsetZ;
    private List<Tweener> _activeTweeners = new List<Tweener>();
    private float _currentThrowingForce;
    private float _throwInputDelayTimer;
    private float _throwingLoadTimer;
    private bool _isLoadingThrow;
    private bool _canThrow;

    public bool IsLoadingThrow => _isLoadingThrow;


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
        _canThrow = false;
    }

    private void TryStartThrowing()
    {
        if (!_canThrow || _isLoadingThrow
            || !_playerManager.GetPlayerHand.IsHoldingThrowable
            || !_playerManager.GetPlayerMovement.CanMove)
            return;

        _playerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Throwing,
            PlayerCamera.Noise.None
        );

        _currentThrowingForce = 0;
        _throwingLoadTimer = 0;
        _isLoadingThrow = true;
    }

    private void UpdateThrowingForce()
    {
        Tweener throwLoadingTween = DOTween.To
        (
            () => _throwingLoadTimer,
            x => _throwingLoadTimer = x,
            1, _throwingLoadDuration
        )
        .SetEase(Ease.OutQuad);

        _activeTweeners.Add(throwLoadingTween);

        _currentThrowingForce = Mathf.Lerp(_minThrowingForce, _maxThrowingForce, _throwingLoadTimer);
        
        PlayerHand playerHand = _playerManager.GetPlayerHand;
        Vector3 throwableOffset = Vector3.Lerp(Vector3.zero, playerHand.transform.forward * _throwingLoadOffsetZ, _throwingLoadTimer);
        
        _playerManager.GetPlayerHand.SetOffsetModifier(throwableOffset);
    }

    private void TryThrow()
    {
        if (!_isLoadingThrow || !_playerManager.GetPlayerHand.IsHoldingThrowable)
            return;

        _playerManager.GetPlayerCamera.SetCameraEffects
        (
            PlayerCamera.FOV.Default,
            PlayerCamera.Noise.None
        );

        _activeTweeners.ForEach((x) => x.Kill());
        _activeTweeners.Clear();

        Throwable throwable = _playerManager.GetPlayerHand.RemoveHeldThrowable();
        ApplyForce(throwable);

        _isLoadingThrow = false;
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
