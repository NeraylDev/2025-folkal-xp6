using UnityEngine;

public class Observer : MonoBehaviour
{

    private PlayerCamera _playerCamera;
    private float _isBeingObserved;
    private bool _isActive;

    private void Start()
    {
        _playerCamera = PlayerManager.instance.GetPlayerCamera;
    }

    private void Update()
    {
        VerifyPlayerView();
    }

    private void VerifyPlayerView()
    {
        if (_playerCamera == null || _isActive == false)
            return;

        Vector3 direction = (transform.position - _playerCamera.transform.position).normalized;
        float dotResult = Vector3.Dot(_playerCamera.transform.forward, direction);

        if (dotResult >= 0.95f)
            Debug.Log("Encontrou observador");
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

}
