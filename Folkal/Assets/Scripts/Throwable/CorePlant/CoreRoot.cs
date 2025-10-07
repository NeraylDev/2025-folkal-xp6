using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreTree : MonoBehaviour, ICoreTree
{
    [SerializeField] protected List<Throwable> _fruitList = new List<Throwable>();

    public List<Throwable> GetFruitList => _fruitList;

    public void AddFruit(Throwable fruit)
    {
        if (_fruitList.Contains(fruit))
            return;

        Debug.Log("Fruto " + fruit.name + " foi adicionado");
        _fruitList.Add(fruit);
    }

    public void RemoveFruit(Throwable fruit)
    {
        if (!_fruitList.Contains(fruit))
            return;

        Debug.Log("Fruto " + fruit.name + " foi removido");
        fruit.RemoveFromRoot();
        _fruitList.Remove(fruit);
    }
}
