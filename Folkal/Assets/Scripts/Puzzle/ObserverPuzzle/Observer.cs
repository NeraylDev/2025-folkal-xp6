using System;
using UnityEngine;

public abstract class Observer : MonoBehaviour
{
    private Transform _playerCameraTransform;
    private bool _wasObserved;
    private bool _isActive;

    private void Update()
    {
        if (_isActive && !_wasObserved)
        {
            VerifyPlayerView();
        }
    }

    protected abstract void OnActivated();
    protected abstract void OnDeactivated();
    protected abstract void OnObserved();

    private void VerifyPlayerView()
    {
        if (_playerCameraTransform == null)
            return;

        Vector3 direction = (transform.position - _playerCameraTransform.position).normalized;
        float dotResult = Vector3.Dot(_playerCameraTransform.forward, direction);

        if (dotResult >= 0.99f)
        {
            OnObserved();
            _wasObserved = true;
        }
    }

    public void Activate(PlayerManager playerManager)
    {
        _playerCameraTransform = playerManager.GetCameraTransform;

        OnActivated();
        _isActive = true;
    }

    public void Deactivate()
    {
        _playerCameraTransform = null;

        OnDeactivated();
        _isActive = false;
    }

}
