using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private bool _showCursor = false;
    [SerializeField] private bool _lockCursor = true;

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

        Cursor.visible = _showCursor;
        Cursor.lockState = _lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void Start()
    {
        _playerController = PlayerController.instance;

        _inputAsset.Player.Move.performed += _playerController.OnMove;
        _inputAsset.Player.Move.canceled += _playerController.OnMove;

        _inputAsset.Player.Interact.started += _playerController.OnInteract;

        _inputAsset.Player.LeftMouse.started += _playerController.OnLeftMouseDown;
        _inputAsset.Player.LeftMouse.canceled += _playerController.OnLeftMouseUp;

        // PARA PROTOTIPAÇÃO
        _inputAsset.Player.R.started += (InputAction.CallbackContext context)
            => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        _inputAsset.Enable();
        _inputAsset.Player.Enable();
    }

    private void OnDisable()
    {
        _inputAsset.Player.Disable();
        _inputAsset.Disable();
    }
}
