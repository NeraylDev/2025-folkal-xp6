using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBreathing : PlayerSubsystem
{
    [SerializeField] private float _breathingDuration;
    private bool _isBreathing;

    public float GetBreathingDuration => _breathingDuration;
    public bool IsBreathing => _isBreathing;

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        actionAsset.FindAction("Breath").started += (InputAction.CallbackContext context)
            => TryBreath();
        actionAsset.FindAction("Breath").canceled += (InputAction.CallbackContext cotext)
            => StopBreath();
    }

    private void TryBreath()
    {
        if (_playerManager.GetPlayerMovement.CanMove == false || _isBreathing)
            return;

        _isBreathing = true;
    }

    private void StopBreath()
    {
        if (!_isBreathing)
            return;

        _isBreathing = false;
    }
}
