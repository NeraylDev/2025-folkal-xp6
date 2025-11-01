using System;
using UnityEngine;

public class BrokenObserver : Observer
{
    protected override void OnActivated()
    {
        Debug.Log("Ativou");
    }

    protected override void OnDeactivated()
    {
        Debug.Log("Desativou");
    }

    protected override void OnObserved()
    {
        Debug.Log("Foi observado");
    }
}
