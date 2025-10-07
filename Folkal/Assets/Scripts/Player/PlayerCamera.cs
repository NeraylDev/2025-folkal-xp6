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

    public void TryInteract()
    {
        Debug.Log("Tenta interagir");

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _interactableMask))
        {
            if (hit.collider.TryGetComponent(out Throwable throwable))
            {
                PlayerHand.instance.SetHeldThrowable(throwable);
            }
        }
    }

}
