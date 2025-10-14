using UnityEngine;

public interface IThrowable
{
    abstract void OnHeld();
    abstract void OnThrown();
    abstract void OnCollide();
}
