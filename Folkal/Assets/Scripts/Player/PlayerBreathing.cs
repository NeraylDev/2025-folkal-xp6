using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBreathing : PlayerSubsystem
{
    private bool _isBreathing;

    public bool IsBreathing => _isBreathing;

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        actionAsset.FindAction("Breath").started += (InputAction.CallbackContext context)
            => _isBreathing = true;
        actionAsset.FindAction("Breath").canceled += (InputAction.CallbackContext cotext)
            => _isBreathing = false;
    }
}
