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
        _rigidBody.useGravity = true;
        _rigidBody.constraints = RigidbodyConstraints.None;
    }

    public void DisableRigidbody()
    {
        _rigidBody.useGravity = false;
        _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
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
