using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : PlayerSubsystem
{
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _interactableMask;
    private Transform _cameraTransform;

    private IInteractable _pointedInteractable;
    private bool _isPointingAtInteractable;

    public bool IsPointingAtInteractable => _isPointingAtInteractable;

    #region MonoBehaviour Methods

    private void Update()
    {
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, _interactionDistance, _interactableMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable == null)
                return;

            if (!_isPointingAtInteractable)
            {
                _playerManager.GetEvents.RaiseStartPointingAtInteractable(interactable);
                _pointedInteractable = interactable;
                _isPointingAtInteractable = true;
            }
        }
        else if (_isPointingAtInteractable)
        {
            _playerManager.GetEvents.RaiseStopPointingAtInteractable();

            _pointedInteractable = null;
            _isPointingAtInteractable = false;
        }
    }

    #endregion


    public override void Initialize(PlayerManager playerManager, InputActionAsset actionAsset = null)
    {
        base.Initialize(playerManager, actionAsset);

        _cameraTransform = playerManager.GetCameraTransform;
    }

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        actionAsset.FindAction("Interact").canceled += (InputAction.CallbackContext context)
            => TryInteract();
    }

    public void TryInteract()
    {
        if (_pointedInteractable == null)
            return;

        _pointedInteractable.Interact(_playerManager);
    }
}
