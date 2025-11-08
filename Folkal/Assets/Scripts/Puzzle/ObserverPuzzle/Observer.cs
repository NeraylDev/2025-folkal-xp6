using System;
using UnityEngine;

public abstract class Observer : MonoBehaviour
{
    protected Transform _playerCameraTransform;
    protected bool _wasObserved;
    protected bool _isActive;

    protected virtual void Update()
    {
        if (_isActive && !_wasObserved)
        {
            VerifyPlayerView();
        }
    }

    protected abstract void OnActivated(PlayerManager playerManager);
    protected abstract void OnDeactivated(PlayerManager playerManager);
    protected abstract void OnObserved();

    private void VerifyPlayerView()
    {
        if (_playerCameraTransform == null)
            return;

        Vector3 direction = (transform.position - _playerCameraTransform.position).normalized;
        float dotResult = Vector3.Dot(_playerCameraTransform.forward, direction);
        dotResult = Mathf.Abs(dotResult);

        if (dotResult >= 0.995f)
        {
            OnObserved();
            _wasObserved = true;
        }
    }

    public void Activate(PlayerManager playerManager)
    {
        _playerCameraTransform = playerManager.GetCameraTransform;

        OnActivated(playerManager);
        _isActive = true;
    }

    public void Deactivate(PlayerManager playerManager)
    {
        _playerCameraTransform = null;

        OnDeactivated(playerManager);
        _isActive = false;
    }

}
