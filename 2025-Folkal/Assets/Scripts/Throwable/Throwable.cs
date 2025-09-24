using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public abstract class Throwable : MonoBehaviour, IThrowable
{
    private CoreRoot _root;

    private Rigidbody _rigidBody;

    private void Awake()
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

    public void SetCoreRoot(CoreRoot root) => _root = root;

    public void RemoveFromRoot()
    {
        if (_root == null) return;

        Debug.Log("Retira do root");

        _root.RemoveFruit(this);
        _root = null;
    }

    public void EnableRigidbody()
    {
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

    public void OnHeld()
    {
        if (_root != null)
            _root.RemoveFruit(this);

        Debug.Log("Segurado");
        DisableRigidbody();
    }

    public void OnThrown()
    {
        Debug.Log("Arremessado");
        EnableRigidbody();
    }

    public abstract void OnCollide();

}
