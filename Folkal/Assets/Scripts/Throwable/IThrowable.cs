using UnityEngine;

public interface IThrowable
{
    void SetParent(Transform newParent);
    void RemoveParent();

    void SetCoreTree(CoreTree root);
    void RemoveFromRoot();

    abstract void OnHeld();
    abstract void OnThrown();
    abstract void OnCollide();
}
