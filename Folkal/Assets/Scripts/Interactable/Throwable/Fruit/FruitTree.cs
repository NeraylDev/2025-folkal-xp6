using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class FruitTree : MonoBehaviour, IFruitTree
{
    [Header("Fruit Tree Info")]
    [SerializeField][ReadOnly(true)] protected List<Fruit> _fruitList = new List<Fruit>();
    
    public List<Fruit> GetFruitList => _fruitList;

    public void AddFruit(Fruit fruit)
    {
        if (_fruitList.Contains(fruit))
            return;

        _fruitList.Add(fruit);
    }

    public void RemoveFruit(Fruit fruit)
    {
        if (!_fruitList.Contains(fruit))
            return;

        fruit.RemoveFromTree();
        _fruitList.Remove(fruit);
    }
}
