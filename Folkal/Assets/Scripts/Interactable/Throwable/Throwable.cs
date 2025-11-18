using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public abstract class Throwable : MonoBehaviour, IInteractable
{
    protected Rigidbody _rigidbody;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.None;

        EnableCollider();
    }

    public void DisableRigidbody()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    protected void EnableCollider()
    {
        if (TryGetComponent(out Collider collider))
            collider.enabled = true;
    }

    protected void DisableCollider()
    {
        if (TryGetComponent(out Collider collider))
            collider.enabled = false;
    }

    public virtual void OnHeld()
    {
        DisableRigidbody();
    }

    public abstract void OnThrown();

    protected abstract void OnCollide();

    private void OnCollisionEnter(Collision collision)
    {
        OnCollide();
    }

}
