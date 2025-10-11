using UnityEngine;

public interface IThrowable
{
    void SetParent(Transform newParent);
    void RemoveParent();

    abstract void OnHeld();
    abstract void OnThrown();
    abstract void OnCollide();
}
