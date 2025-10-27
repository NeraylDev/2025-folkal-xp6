using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerHand _playerHand;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private PlayerInteraction _playerInteraction;

    public PlayerMovement GetPlayerMovement => _playerMovement;
    public PlayerHand GetPlayerHand => _playerHand;
    public PlayerCamera GetPlayerCamera => _playerCamera;
    public PlayerInteraction GetPlayerInteraction => _playerInteraction;


    public static PlayerController instance;


    #region MonoBehaviour Methods

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        InputActionAsset actionAsset = InputSystem.actions;
        _playerCamera.Initialize(this, actionAsset);
        _playerHand.Initialize(this, actionAsset);
        _playerInteraction.Initialize(this, actionAsset);
        _playerMovement.Initialize(this, actionAsset);
    }

    #endregion

}
