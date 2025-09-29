using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private Transform _cameraPoint;

    private PlayerController _playerController;

    public static PlayerCamera instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        _playerController = PlayerController.instance;
    }

    private void Update()
    {
        //_cameraPoint = _playerController
    }
}
