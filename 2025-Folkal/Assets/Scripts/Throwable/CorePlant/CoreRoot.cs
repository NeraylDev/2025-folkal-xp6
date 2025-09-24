using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreRoot : MonoBehaviour, ICoreRoot
{
    [SerializeField] private List<Throwable> _fruitList = new List<Throwable>();

    public void RemoveFruit(Throwable fruit)
    {
        if (!_fruitList.Contains(fruit)) return;

        Debug.Log("Fruto " + fruit.name + " foi removido");
        fruit.RemoveFromRoot();
        _fruitList.Remove(fruit);
    }
}
