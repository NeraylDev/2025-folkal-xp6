using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public abstract class Throwable : MonoBehaviour, IThrowable
{
    protected CoreTree _root;

    private Rigidbody _rigidBody;

    public CoreTree GetRoot => _root;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        if (transform.parent.TryGetComponent(out CoreTree root))
        {
            SetCoreTree(root);
        }
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

    public void SetCoreTree(CoreTree root) => _root = root;

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

    public virtual void OnHeld()
    {
        Debug.Log("Segurado");
        DisableRigidbody();
    }

    public virtual void OnThrown()
    {
        Debug.Log("Arremessado");
        EnableRigidbody();
    }

    public abstract void OnCollide();

    private void OnCollisionEnter(Collision collision)
    {
        OnCollide();
    }

}
