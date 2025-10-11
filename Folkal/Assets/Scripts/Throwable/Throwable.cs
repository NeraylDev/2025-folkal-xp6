using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public abstract class Throwable : MonoBehaviour, IThrowable
{
    protected Rigidbody _rigidBody;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void SetParent(Transform newParent)
    {
        DisableRigidbody();
        transform.parent = newParent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void RemoveParent()
    {
        EnableRigidbody();
        transform.parent = null;
    }

    public void EnableRigidbody()
    {
        _rigidBody.angularVelocity = Vector3.zero;
        _rigidBody.linearVelocity = Vector3.zero;

        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = true;
        _rigidBody.constraints = RigidbodyConstraints.None;

        if (TryGetComponent(out Collider collider))
            collider.enabled = true;
    }

    public void DisableRigidbody()
    {
        _rigidBody.isKinematic = true;
        _rigidBody.useGravity = false;
        _rigidBody.constraints = RigidbodyConstraints.FreezeAll;

        if (TryGetComponent(out Collider collider))
            collider.enabled = false;
    }

    public virtual void OnHeld()
    {
        Debug.Log("Segurado");
        DisableRigidbody();
    }

    public virtual void OnThrown()
    {
        Debug.Log("Arremessado");
    }

    public abstract void OnCollide();

    private void OnCollisionEnter(Collision collision)
    {
        OnCollide();
    }

}
