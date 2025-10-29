using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private Material _activeMaterial;
    private PlayerCamera _playerCamera;
    private bool _wasObserved;
    private bool _isActive;

    private void Start()
    {
        _playerCamera = PlayerManager.instance.GetPlayerCamera;
    }

    private void Update()
    {
        if (_isActive && !_wasObserved)
        {
            VerifyPlayerView();
        }
    }

    private void VerifyPlayerView()
    {
        if (_playerCamera == null)
            return;

        Vector3 direction = (transform.position - _playerCamera.transform.position).normalized;
        float dotResult = Vector3.Dot(_playerCamera.transform.forward, direction);

        if (dotResult >= 0.95f)
        {
            GetComponent<Renderer>().material = _activeMaterial;
            Debug.Log("Revela ambiente");
            _wasObserved = true;
        }
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
