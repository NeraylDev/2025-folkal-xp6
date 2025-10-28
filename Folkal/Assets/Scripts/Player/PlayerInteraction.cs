using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : PlayerSubsystem
{
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _interactableMask;

    protected override void SetEvents(InputActionAsset actionAsset)
    {
        actionAsset.FindAction("Interact").canceled += (InputAction.CallbackContext context)
            => TryInteract();
    }

    public void TryInteract()
    {
        Transform cameraTransform = Camera.main.transform;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, _interactionDistance, _interactableMask))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
                interactable.Interact(_playerManager);
        }
    }
}
