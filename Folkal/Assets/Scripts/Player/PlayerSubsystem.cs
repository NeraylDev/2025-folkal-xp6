using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerSubsystem : MonoBehaviour
{
    protected PlayerManager _playerManager;
    public PlayerManager GetPlayerManager => _playerManager;

    public virtual void Initialize(PlayerManager playerManager, InputActionAsset actionAsset)
    {
        if (_playerManager != null)
            return;

        _playerManager = playerManager;

        if (actionAsset != null)
            SetEvents(actionAsset);
    }

    protected virtual void SetEvents(InputActionAsset actionAsset) { }
}
