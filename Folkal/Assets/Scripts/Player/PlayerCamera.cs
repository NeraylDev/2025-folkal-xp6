using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _interactableMask;

    public static PlayerCamera instance;
    
    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Update()
    {
        UpdateCameraRaycast();
    }

    private void UpdateCameraRaycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _interactableMask)
            && !PlayerHand.instance.IsHoldingThrowable)
        {
            HUDManager.instance.SetCrosshair(HUDManager.CrosshairType.Interaction);
        }
        else
        {
            HUDManager.instance.SetCrosshair(HUDManager.CrosshairType.Default);
        }
    }

    public void TryInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _interactableMask))
        {
            if (hit.collider.TryGetComponent(out Throwable throwable))
            {
                PlayerHand.instance.SetHeldThrowable(throwable);
            }
            else if (hit.collider.TryGetComponent(out NPC npc))
            {
                npc.TryStartDialogue();
            }
            else if (hit.collider.TryGetComponent(out Sign sign))
            {
                sign.TryStartReading();
            }
        }
    }

}
