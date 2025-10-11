using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class FruitTree : MonoBehaviour, IFruitTree
{
    [Header("Fruit Tree Info")]
    [SerializeField][ReadOnly(true)] protected List<Throwable> _fruitList = new List<Throwable>();
    
    public List<Throwable> GetFruitList => _fruitList;

    public void AddFruit(Fruit fruit)
    {
        if (_fruitList.Contains(fruit))
            return;

        Debug.Log("Fruto " + fruit.name + " foi adicionado");
        _fruitList.Add(fruit);
    }

    public void RemoveFruit(Fruit fruit)
    {
        if (!_fruitList.Contains(fruit))
            return;

        Debug.Log("Fruto " + fruit.name + " foi removido");
        fruit.RemoveFromTree();
        _fruitList.Remove(fruit);
    }
}
