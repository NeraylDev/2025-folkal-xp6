using UnityEngine;

public class EnergyTree : CoreTree
{

    private void Awake()
    {
        ConnectFruits();
    }

    private void ConnectFruits()
    {
        if (GetComponentsInChildren<EnergyFruit>() == null)
            return;

        EnergyFruit[] fruits = GetComponentsInChildren<EnergyFruit>();
        for (int i = 0; i < fruits.Length; i++)
        {
            AddFruit(fruits[i]);

            // Define qual é o NextSibling do fruto
            if (i + 1 < fruits.Length)
                fruits[i].SetNextSibling(fruits[i + 1]);
            else
                fruits[i].SetNextSibling(fruits[0]);

            // Define qual é o PreviousSibling do fruto
            if (i == 0)
                fruits[i].SetPreviousSibling(fruits[fruits.Length - 1]);
            else
                fruits[i].SetPreviousSibling(fruits[i - 1]);
        }
    }

    public void ResetFruitsPosition()
    {
        foreach (EnergyFruit fruit in _fruitList)
            fruit.ResetPosition();
    }

    public bool isAnyFruitActive()
    {
        foreach (EnergyFruit fruit in _fruitList)
        {
            if (fruit.IsActive)
                return true;
        }

        return false;
    }

    public bool isAllFruitsActive()
    {
        foreach (EnergyFruit fruit in _fruitList)
        {
            if (!fruit.IsActive)
                return false;
        }

        return true;
    }

}
