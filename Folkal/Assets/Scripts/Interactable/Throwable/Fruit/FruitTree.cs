using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class FruitTree : MonoBehaviour, IFruitTree
{
    [Header("Fruit Tree Info")]
    [SerializeField][ReadOnly(true)] protected List<Fruit> _fruitList = new List<Fruit>();

    protected Action<Fruit> onAddFruit;
    protected Action<Fruit> onRemoveFruit;

    public List<Fruit> GetFruitList => _fruitList;

    public void AddFruit(Fruit fruit)
    {
        if (_fruitList.Contains(fruit))
            return;

        _fruitList.Add(fruit);
        onAddFruit?.Invoke(fruit);
    }

    public void RemoveFruit(Fruit fruit)
    {
        if (!_fruitList.Contains(fruit))
            return;

        _fruitList.Remove(fruit);
        onRemoveFruit?.Invoke(fruit);
    }
}
