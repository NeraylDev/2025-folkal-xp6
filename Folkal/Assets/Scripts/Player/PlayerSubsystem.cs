using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerSubsystem : MonoBehaviour
{
    protected PlayerController _playerController;
    public PlayerController GetPlayerController => _playerController;

    public void Initialize(PlayerController playerController, InputActionAsset actionAsset)
    {
        if (_playerController != null)
            return;

        _playerController = playerController;

        if (actionAsset != null)
            SetEvents(actionAsset);
    }

    protected abstract void SetEvents(InputActionAsset actionAsset);
}
