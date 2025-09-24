using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        _mainCameraTransform = PlayerCamera.instance.transform;
    }

    private void Update()
    {
        if (_mainCameraTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, _handPosition.position, Time.deltaTime * _handPositionSpeed);
        transform.LookAt(transform.position + _mainCameraTransform.forward);
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
        throwable.transform.parent = transform;
        _heldThrowable = throwable;
    }

    public void RemoveHeldThrowable()
    {
        if (_heldThrowable == null) return;

        Debug.Log("Parou de segurar " + _heldThrowable.name);
        _heldThrowable.transform.parent = null;
        _heldThrowable = null;
    }

}
