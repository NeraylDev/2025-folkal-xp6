using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerEvents _playerEvents;

    [Header("Player Subsystems")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerHand _playerHand;
    [SerializeField] private PlayerThrowing _playerThrowing;

    private PlayerStateMachine _playerStateMachine;
    private InputActionAsset _playerInputActions;

    public PlayerEvents GetEvents => _playerEvents;

    public PlayerMovement GetPlayerMovement => _playerMovement;
    public PlayerCamera GetPlayerCamera => _playerCamera;
    public PlayerInteraction GetPlayerInteraction => _playerInteraction;
    public PlayerHand GetPlayerHand => _playerHand;
    public PlayerThrowing GetPlayerThrowing => _playerThrowing;

    public PlayerStateMachine GetPlayerStateMachine => _playerStateMachine;
    public InputActionAsset GetPlayerInputActions => _playerInputActions;

    public static PlayerManager instance;


    #region MonoBehaviour Methods

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        // --- Subsystems Initialization ---
        InputActionAsset actionAsset = InputSystem.actions;
        _playerMovement.Initialize(this, actionAsset);
        _playerCamera.Initialize(this, actionAsset);
        _playerInteraction.Initialize(this, actionAsset);
        _playerHand.Initialize(this);
        _playerThrowing.Initialize(this, actionAsset);

        // --- State Machine ---
        _playerStateMachine = new PlayerStateMachine();
        _playerStateMachine.Initialize(new PlayerIdleState(_playerStateMachine, this));
    
        // --- InputActions ---
        _playerInputActions = InputSystem.actions;
    }

    private void Update()
    {
        _playerStateMachine.Execute();
    }

    private void FixedUpdate()
    {
        _playerStateMachine.FixedExecute();
    }

    #endregion


    #region Action Methods

    public void PickUpThrowable(Throwable throwable)
    {
        if (CanPickUpThrowable() == false)
            return;

        _playerHand.SetHeldThrowable(throwable);
        _playerThrowing.ResetInputDelayTimer();

        _playerEvents.RaisePickUpThrowable(this);
    }

    public Throwable DropThrowable()
    {
        if (CanDropThrowable() == false)
            return null;

        _playerEvents.RaiseDropThrowable(this);

        return _playerHand.RemoveHeldThrowable();
    }

    #endregion


    #region Condition Methods

    public bool CanPickUpThrowable()
    {
        return !_playerHand.IsHoldingThrowable;
    }

    public bool CanDropThrowable()
    {
        return _playerHand.IsHoldingThrowable;
    }

    public bool CanStartDialogue()
    {
        return !_playerThrowing.IsLoadingThrow;
    }

    public bool CanReadSign()
    {
        return !_playerThrowing.IsLoadingThrow;
    }

    #endregion

}
