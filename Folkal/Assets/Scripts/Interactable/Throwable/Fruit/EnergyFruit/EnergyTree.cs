using DG.Tweening;
using UnityEngine;

public class EnergyTree : FruitTree
{

    [Header("Treetop Settings")]
    [SerializeField] private Transform _treetop;
    [SerializeField] private Transform _treetopRig;
    [SerializeField] private float _treetopMinHeight;
    [Space]
    [SerializeField] private float _treetopMovementDuration = 0.25f;
    [SerializeField] private float _treetopMovementDelay = 0.05f;
    [SerializeField] private Ease _treetopMovementCurve;
    private Bounds _fruitsBound;

    public Transform GetTreetop => _treetop;
    public Transform GetTreetopRig => _treetopRig;

    private void Awake()
    {
        ConnectFruits();
    }

    private void Update()
    {
        UpdateTreetopPosition();
    }

    private void UpdateTreetopPosition()
    {
        Vector3 finalPosition = GetFruitsCenterPoint();
        if (finalPosition.y < _treetopMinHeight)
            finalPosition = new Vector3(finalPosition.x, _treetopMinHeight, finalPosition.z);

        _treetop.DOMove(finalPosition, _treetopMovementDuration).SetEase(_treetopMovementCurve).SetDelay(_treetopMovementDelay);
    }

    private Vector3 GetFruitsCenterPoint()
    {
        _fruitsBound = new Bounds(_fruitList[0].transform.position, Vector3.zero);
        for (int i = 1; i < _fruitList.Count; i++)
        {
            _fruitsBound.Encapsulate(_fruitList[i].transform.position);
        }

        return _fruitsBound.center;
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
