using UnityEngine;

public class Observer : MonoBehaviour
{

    private PlayerCamera _playerCamera;
    private float _isBeingObserved;
    private bool _isActive;

    private void Start()
    {
        _playerCamera = PlayerCamera.instance;
    }

    private void Update()
    {
        VerifyPlayerView();
    }

    private void VerifyPlayerView()
    {
        if (_playerCamera == null || _isActive)
            return;

        Vector3 direction = (transform.position - _playerCamera.transform.position).normalized;
        float dotResult = Vector3.Dot(_playerCamera.transform.forward, direction);

        if (dotResult >= 0.95f)
            OnActivate();
    }

    private void OnActivate()
    {
        
    }

}
