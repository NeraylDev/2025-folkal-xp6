using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public abstract class Throwable : MonoBehaviour, IInteractable
{
    protected Rigidbody _rigidBody;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerManager playerManager)
    {
        playerManager.GetPlayerHand.PickUpThrowable(this);
    }

    public void EnableRigidbody()
    {
        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = true;
        _rigidBody.angularVelocity = Vector3.zero;
        _rigidBody.linearVelocity = Vector3.zero;
        _rigidBody.constraints = RigidbodyConstraints.None;

        EnableCollider();
    }

    public void DisableRigidbody()
    {
        _rigidBody.isKinematic = true;
        _rigidBody.useGravity = false;
        _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void EnableCollider()
    {
        if (TryGetComponent(out Collider collider))
            collider.enabled = true;
    }

    public void DisableCollider()
    {
        if (TryGetComponent(out Collider collider))
            collider.enabled = false;
    }

    public virtual void OnHeld()
    {
        DisableRigidbody();
    }

    public abstract void OnThrown();

    public abstract void OnCollide();

    private void OnCollisionEnter(Collision collision)
    {
        OnCollide();
    }

}
