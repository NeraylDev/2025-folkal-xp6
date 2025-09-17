using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _moveDirection;

    public Vector2 GetMoveDirection => _moveDirection;

    public static PlayerController instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        Debug.Log(_moveDirection);
    }

    public void OnStartRun(InputAction.CallbackContext context) { Debug.Log("Start Run"); }
    public void OnStopRun(InputAction.CallbackContext context) { Debug.Log("Stop Run"); }

    public void OnInteract(InputAction.CallbackContext context) { Debug.Log("Interact"); }

    public void OnLeftShiftDown(InputAction.CallbackContext context) { Debug.Log("LeftShift Down"); }
    public void OnLeftShiftUp(InputAction.CallbackContext context) { Debug.Log("LeftShift Up"); }

    public void OnLeftMouseDown(InputAction.CallbackContext context) { Debug.Log("Left Mouse Down"); }
    public void OnLeftMouseUp(InputAction.CallbackContext context) { Debug.Log("Left Mouse Up"); }

    public void OnRightMouseDown(InputAction.CallbackContext context) { Debug.Log("Right Mouse Down"); }
    public void OnRightMouseUp(InputAction.CallbackContext context) { Debug.Log("Right Mouse Up"); }


}
