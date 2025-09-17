using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerController _playerController;

    private PlayerInputAsset _inputAsset;

    private void Awake()
    {
        _inputAsset = new PlayerInputAsset();
        _playerController = PlayerController.instance;
    }

    private void Start()
    {
        _inputAsset.Player.Move.performed += _playerController.OnMove;

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
