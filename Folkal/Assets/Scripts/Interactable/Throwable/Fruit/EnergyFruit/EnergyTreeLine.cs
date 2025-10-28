using UnityEngine;

public class EnergyTreeLine : LineManager
{
    private EnergyTree _energyTree;
    private EnergyFruit _fruit;

    private PlayerManager _playerManager;

    private void Awake()
    {
        _energyTree = GetComponentInParent<EnergyTree>();
        _fruit = GetComponent<EnergyFruit>();
    }

    private void Start()
    {
        _playerManager = PlayerManager.instance;
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

            if (_playerManager.GetPlayerHand.IsHoldingThrowable && _playerManager.GetPlayerHand.GetHeldThrowable == _fruit)
            {
                _playerManager.GetPlayerHand.RemoveHeldThrowable();
                _playerManager.GetPlayerMovement.ResetMoveSpeedModifier();
            }
        }
    }
        
}
