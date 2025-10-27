using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerHand _playerHand;
    [SerializeField] private PlayerThrowing _playerThrowing;

    public PlayerMovement GetPlayerMovement => _playerMovement;
    public PlayerCamera GetPlayerCamera => _playerCamera;
    public PlayerInteraction GetPlayerInteraction => _playerInteraction;
    public PlayerHand GetPlayerHand => _playerHand;
    public PlayerThrowing GetPlayerThrowing => _playerThrowing;

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
        _playerHand.Initialize(this, actionAsset);
        _playerThrowing.Initialize(this, actionAsset);
    }

    #endregion

}
