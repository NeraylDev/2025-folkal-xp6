using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private Material _activeMaterial;
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

    private void VerifyPlayerView()
    {
        if (_playerCameraTransform == null)
            return;

        Vector3 direction = (transform.position - _playerCameraTransform.position).normalized;
        float dotResult = Vector3.Dot(_playerCameraTransform.forward, direction);

        if (dotResult >= 0.985f)
        {
            GetComponent<Renderer>().material = _activeMaterial;
            Debug.Log("Revela ambiente");
            _wasObserved = true;
        }
    }

    public void Activate(PlayerManager playerManager)
    {
        _playerCameraTransform = playerManager.GetCameraTransform;
        _isActive = true;
    }

    public void Deactivate()
    {
        _playerCameraTransform = null;
        _isActive = false;
    }

}
