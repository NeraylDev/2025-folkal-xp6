using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : PlayerSubsystem
{
    private InputActionAsset _inputActions;

    private void Awake()
    {
        _inputActions = InputSystem.actions;
    }

    private void Start()
    {
        _inputActions.FindAction("Run").started += (InputAction.CallbackContext context)
            => _playerManager.GetEvents.RaiseRunStart(_playerManager);
        _inputActions.FindAction("Run").canceled += (InputAction.CallbackContext context)
            => _playerManager.GetEvents.RaiseRunStop(_playerManager);

        _inputActions.FindAction("Interact").started += (InputAction.CallbackContext context)
            => _playerManager.GetEvents.RaiseInteract(_playerManager);

        _inputActions.FindAction("Breath").started += (InputAction.CallbackContext context)
            => _playerManager.GetEvents.RaiseBreathingStart(_playerManager);
        _inputActions.FindAction("Breath").canceled += (InputAction.CallbackContext context)
            => _playerManager.GetEvents.RaiseBreathingStop(_playerManager);
    }

}
