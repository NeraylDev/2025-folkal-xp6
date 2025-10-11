using UnityEngine;

public class EnergyTreeLine : LineManager
{
    private EnergyTree _energyTree;

    private PlayerHand _playerHand;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _energyTree = GetComponentInParent<EnergyTree>();
    }

    private void Start()
    {
        _playerHand = PlayerHand.instance;
        _playerMovement = PlayerMovement.instance;
    }

    private void Update()
    {
        UpdateLinePosition(transform.position, _energyTree.GetTreetopRig.position);
    }

    protected override void CalculateLineLength()
    {
        float distanceToFruit = Vector3.Distance
        (
            transform.position,
            _energyTree.GetTreetopRig.transform.position
        );

        if (distanceToFruit >= _maxLineLength)
            OnReachMaxLength();
    }

    protected override void OnReachMaxLength()
    {
        if (!_energyTree.isAnyFruitActive())
        {
            _energyTree.ResetFruitsPosition();

            if (_playerHand.IsHoldingThrowable)
            {
                _playerHand.RemoveHeldThrowable();
                _playerMovement.ResetMoveSpeedModifier();
            }
        }
    }
        
}
