using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _moveDirection;
    private Vector2 _mouseDelta;

    private PlayerCamera _playerCamera;
    private PlayerHand _playerHand;

    public Vector2 GetMoveDirection => _moveDirection;
    public Vector2 GetMouseDelta => _mouseDelta;

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
        _playerCamera = PlayerCamera.instance;
        _playerHand = PlayerHand.instance;
    }

    public void OnMove(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();

    public void OnStartRun(InputAction.CallbackContext context) { Debug.Log("Start Run"); }
    public void OnStopRun(InputAction.CallbackContext context) { Debug.Log("Stop Run"); }

    public void OnInteract(InputAction.CallbackContext context)
    {
        _playerCamera.TryInteract();
    }

    public void OnMouseDelta(InputAction.CallbackContext context) { Debug.Log("Mouse Delta"); }

    public void OnLeftShiftDown(InputAction.CallbackContext context) { Debug.Log("LeftShift Down"); }
    public void OnLeftShiftUp(InputAction.CallbackContext context) { Debug.Log("LeftShift Up"); }

    public void OnLeftMouseDown(InputAction.CallbackContext context)
    {
        _playerCamera.TryInteract();
        _playerHand.TryStartThrowing();
    }
    public void OnLeftMouseUp(InputAction.CallbackContext context) { _playerHand.TryThrow(); }

    public void OnRightMouseDown(InputAction.CallbackContext context) { Debug.Log("Right Mouse Down"); }
    public void OnRightMouseUp(InputAction.CallbackContext context) { Debug.Log("Right Mouse Up"); }


}
