using UnityEngine;

public class EnergyTreeLine : LineManager
{
    private EnergyTree _energyTree;
    private EnergyFruit _fruit;

    private PlayerHand _playerHand;
    private PlayerManager _playerMovement;

    private void Awake()
    {
        _energyTree = GetComponentInParent<EnergyTree>();
        _fruit = GetComponent<EnergyFruit>();
    }

    private void Start()
    {
        _playerHand = PlayerHand.instance;
        _playerMovement = PlayerManager.instance;
    }

    private void Update()
    {
        UpdateLinePosition(transform.position, _energyTree.GetTreetopRig.position);
    }

    protected override void CalculateLineLength()
    {
        Vector3 treetopRigPosition = _energyTree.GetTreetopRig.transform.position;
        float distanceToTree = Vector3.Distance(transform.position, treetopRigPosition);

        if (distanceToTree >= _maxLineLength)
            OnReachMaxLength();
    }

    protected override void OnReachMaxLength()
    {
        if (!_energyTree.isAnyFruitActive())
        {
            _energyTree.ResetFruitsPosition();

            if (_playerHand.IsHoldingThrowable && _playerHand.GetHeldThrowable == _fruit)
            {
                _playerHand.RemoveHeldThrowable();
                _playerMovement.GetPlayerMovement.ResetMoveSpeedModifier();
            }
        }
    }
        
}
