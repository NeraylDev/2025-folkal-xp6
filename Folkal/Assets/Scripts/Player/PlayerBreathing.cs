using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBreathing : PlayerSubsystem
{
    [SerializeField] private float _breathingInDuration;
    [SerializeField] private float _breathingOutDuration;
    private float _breathingDuration;
    private bool _isBreathing;
    private bool _isBreathingIn;
    private bool _isBreathingOut;
    private bool _canBreathOut;

    public float GetBreathingDuration => _breathingDuration;
    public float GetBreathingInDuration => _breathingInDuration;
    public float GetBreathingOutDuration => _breathingOutDuration;
    public bool IsBreathing => _isBreathing;
    public bool IsBreathingIn => _isBreathingIn;
    public bool IsBreathingOut => _isBreathingOut;
    public bool CanBreathOut => _canBreathOut;

    public void SetIsBreathing(bool value) => _isBreathing = value;
    public void SetCanBreathOut(bool value) => _canBreathOut = value;

    public override void Initialize(PlayerManager playerManager, InputActionAsset actionAsset = null)
    {
        base.Initialize(playerManager, actionAsset);

        _breathingDuration = _breathingInDuration + _breathingOutDuration;
    }

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        actionAsset.FindAction("Breath").started += (InputAction.CallbackContext context)
            => TryBreath();
        actionAsset.FindAction("Breath").canceled += (InputAction.CallbackContext cotext)
            => TryStopBreath();
    }

    private void TryBreath()
    {
        if (_playerManager.GetPlayerMovement.CanMove == false || _isBreathing)
            return;

        _isBreathing = true;
    }

    private void TryStopBreath()
    {
        if (!_isBreathing)
            return;
        
        if (_canBreathOut)
        {
            _isBreathingOut = true;
        }
        else
        {
            _playerManager.GetEvents.RaiseBreathingCanceled(_playerManager);
            StopBreath();
        }
    }

    public void StopBreath()
    {
        _canBreathOut = false;
        _isBreathingIn = false;
        _isBreathingOut = false;
        _isBreathing = false;
    }
}
