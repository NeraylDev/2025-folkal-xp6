using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    private CinemachineVirtualCamera _virtualCamera;

    private PlayerController _playerController;

    public static PlayerCamera instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        _playerController = PlayerController.instance;
    }
}
