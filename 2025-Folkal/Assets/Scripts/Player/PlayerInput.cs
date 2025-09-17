using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerInputAsset _inputAsset;

    public static PlayerInput instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        _inputAsset = new PlayerInputAsset();
    }

    private void Start()
    {
        _playerController = PlayerController.instance;

        _inputAsset.Player.Move.performed += _playerController.OnMove;
        _inputAsset.Player.Move.canceled += _playerController.OnMove;

        _inputAsset.Player.Interact.started += _playerController.OnInteract;

        _inputAsset.Player.Shift.performed += _playerController.OnStartRun;
        _inputAsset.Player.Shift.canceled += _playerController.OnStopRun;

        _inputAsset.Player.LeftMouse.started += _playerController.OnLeftMouseDown;
        _inputAsset.Player.LeftMouse.canceled += _playerController.OnLeftMouseUp;

        _inputAsset.Player.RightMouse.started += _playerController.OnRightMouseDown;
        _inputAsset.Player.RightMouse.canceled += _playerController.OnRightMouseUp;

        _inputAsset.Enable();
        _inputAsset.Player.Enable();
    }
}
