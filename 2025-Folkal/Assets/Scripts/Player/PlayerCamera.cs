using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachinePOV _cinemachinePOV;

    private PlayerController _playerController;

    public static PlayerCamera instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachinePOV = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Start()
    {
        _playerController = PlayerController.instance;
    }

    private void Update()
    {
        _cinemachinePOV.m_VerticalAxis.m_MaxSpeed = _playerController.GetMouseSensibility.y;
    }
}
