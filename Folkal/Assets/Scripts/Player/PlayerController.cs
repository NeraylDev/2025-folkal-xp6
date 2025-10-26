using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _moveDirection;
    private Vector2 _mouseDelta;

    private PlayerCamera _playerCamera;
    private PlayerHand _playerHand;
    private PlayerMovement _playerMovement;

    public Vector2 GetMoveDirection => _moveDirection;
    public Vector2 GetMouseDelta => _mouseDelta;

    public static PlayerController instance;

    [HideInInspector] public UnityEvent onStartRun;
    [HideInInspector] public UnityEvent onStopRun;

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
        _playerMovement = PlayerMovement.instance;
    }

    public void OnMove(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();
    public void OnActivateRunning(InputAction.CallbackContext context)
    {
        if (!_playerMovement.CanMove || _moveDirection == Vector2.zero || _playerHand.IsLoadingThrow
            || Vector2.Dot(_moveDirection, Vector2.up) <= 0)
            return;

        _playerMovement.SetIsRunning(true);
        onStartRun.Invoke();
    }

    public void OnDeactivateRunning(InputAction.CallbackContext context)
    {
        if (_playerHand.IsLoadingThrow)
            return;

        _playerMovement.SetIsRunning(false);
        onStopRun.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (_playerHand.IsLoadingThrow)
            return;

        _playerCamera.TryInteract();

        DialogueUI dialogueUI = DialogueUI.instance;
        if (dialogueUI != null)
            dialogueUI.TryUpdateSpeech();

        SignUI signUI = SignUI.instance;
        if (signUI != null)
            signUI.TryUpdateSpeech();
    }

    public void OnLeftMouseDown(InputAction.CallbackContext context)
    {
        _playerCamera.TryInteract();
        _playerHand.TryStartThrowing();
    }

    public void OnLeftMouseUp(InputAction.CallbackContext context)
    {
        _playerHand.TryThrow();
    }

}
