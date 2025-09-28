using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 _mouseSensibility;
    private Vector2 _moveDirection;

    private PlayerMovement _playerMovement;
    private PlayerHand _playerHand;
    private PlayerCamera _playerCamera;

    public Vector2 GetMoveDirection => _moveDirection;
    public Vector2 GetMouseSensibility => _mouseSensibility;

    public static PlayerController instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        _playerMovement = PlayerMovement.instance;
        _playerHand = PlayerHand.instance;
        _playerCamera = PlayerCamera.instance;
    }

    public void OnMove(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();

    public void OnStartRun(InputAction.CallbackContext context) { Debug.Log("Start Run"); }
    public void OnStopRun(InputAction.CallbackContext context) { Debug.Log("Stop Run"); }

    public void OnInteract(InputAction.CallbackContext context)
    {
        _playerHand.TryInteract();
    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        _playerMovement.RotateY(context.ReadValue<Vector2>().x);
    }

    public void OnLeftShiftDown(InputAction.CallbackContext context) { Debug.Log("LeftShift Down"); }
    public void OnLeftShiftUp(InputAction.CallbackContext context) { Debug.Log("LeftShift Up"); }

    public void OnLeftMouseDown(InputAction.CallbackContext context) { Debug.Log("Left Mouse Down"); }
    public void OnLeftMouseUp(InputAction.CallbackContext context) { Debug.Log("Left Mouse Up"); }

    public void OnRightMouseDown(InputAction.CallbackContext context) { Debug.Log("Right Mouse Down"); }
    public void OnRightMouseUp(InputAction.CallbackContext context) { Debug.Log("Right Mouse Up"); }


}
