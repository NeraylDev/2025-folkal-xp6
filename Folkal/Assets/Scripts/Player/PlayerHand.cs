using System.Collections;
using System.Collections.Generic;
//using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform _handPosition;
    [SerializeField] private float _handPositionSpeed;
    [Space]
    [SerializeField] private Throwable _heldThrowable;

    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance = 10f;
    [SerializeField] private LayerMask _interactableMask;

    private Transform _mainCameraTransform;

    public static PlayerHand instance;

    private void Awake()
    {
        // --- Singleton ---
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (_mainCameraTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, _handPosition.position, Time.fixedDeltaTime * _handPositionSpeed);
        //transform.DOLocalMove(_handPosition.position, 0.1f);
        transform.LookAt(transform.position + _mainCameraTransform.forward);
        //transform.DOLookAt(transform.position + _mainCameraTransform.forward, 0.1f);
    }

    public void TryInteract()
    {
        Debug.Log("Tenta interagir");

        if (Physics.Raycast(_mainCameraTransform.position, _mainCameraTransform.forward, out RaycastHit hit, _interactionDistance, _interactableMask))
        {
            Debug.Log("Interagiu com o objeto " + hit.collider.name);

            if (hit.collider.TryGetComponent(out Throwable throwable))
            {
                SetHeldThrowable(throwable);
            }
        }
    }

    public void SetHeldThrowable(Throwable throwable)
    {
        if (_heldThrowable != null) return;

        Debug.Log("Começou a segurar " + throwable.name);
        throwable.SetParent(transform);
        _heldThrowable = throwable;
    }

    public void RemoveHeldThrowable()
    {
        if (_heldThrowable == null) return;

        Debug.Log("Parou de segurar " + _heldThrowable.name);
        _heldThrowable.RemoveParent();
        _heldThrowable = null;
    }


    private void OnDrawGizmos()
    {
        if (_mainCameraTransform == null) return;

        Gizmos.DrawLine(_mainCameraTransform.position, _mainCameraTransform.position + _mainCameraTransform.forward * _interactionDistance);
    }
}
