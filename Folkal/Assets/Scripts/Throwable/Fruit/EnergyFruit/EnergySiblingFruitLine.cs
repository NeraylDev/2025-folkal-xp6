using System.Collections.Generic;
using UnityEngine;

public class EnergySiblingFruitLine : LineManager
{
    private EnergyFruit _fruit;

    private PlayerHand _playerHand;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _fruit = GetComponent<EnergyFruit>();
    }

    private void Start()
    {
        _playerHand = PlayerHand.instance;
        _playerMovement = PlayerMovement.instance;
    }

    private void Update()
    {
        if (_fruit.GetNextSibling == null)
            return;

        UpdateLinePosition(transform.position, _fruit.GetNextSibling.transform.position);
        FindEnergyTrigger();
    }

    protected override void CalculateLineLength()
    {
        Vector3 previousSiblingPosition = _fruit.GetPreviousSibling.transform.position;
        Vector3 nextSiblingPosition = _fruit.GetNextSibling.transform.position;

        float distanceToPreviousSibling = Vector3.Distance(transform.position, previousSiblingPosition);
        float distanceToNextSibling = Vector3.Distance(transform.position, nextSiblingPosition);
        float greaterDistanceToSibling = distanceToPreviousSibling > distanceToNextSibling ? distanceToPreviousSibling : distanceToNextSibling;

        if (_playerHand.IsHoldingThrowable && _playerHand.GetHeldThrowable == _fruit)
        {
            // Modifica velocidade do Player de acordo com a distância até SiblingFruit
            float speedModifier = Mathf.Lerp(1, 0.25f, distanceToNextSibling / _maxLineLength);

            // Define o uso do modificador de acordo com a direção que o Player está se movendo
            Vector3 fruitToSiblingDirection = nextSiblingPosition - transform.position;
            Vector3 playerMovementDirection = _playerMovement.GetMoveDirection;
            float directionModifier = Mathf.Clamp01(Vector3.Dot(playerMovementDirection, fruitToSiblingDirection));

            _playerMovement.SetMoveSpeedModifier(Mathf.Lerp(speedModifier, 1, directionModifier));

            if (greaterDistanceToSibling > _maxLineLength)
                OnReachMaxLength();
        }
    }

    protected override void OnReachMaxLength()
    {
        _playerHand.RemoveHeldThrowable();
        _playerMovement.ResetMoveSpeedModifier();
    }

    private void FindEnergyTrigger()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, _fruit.GetNextSibling.transform.position, out hit))
        {
            if (hit.collider.CompareTag("EnergyTrigger") && hit.collider.TryGetComponent(out EnergyTrigger energyTrigger))
                ActivateEnergyTrigger(energyTrigger);
        }
    }

    private void ActivateEnergyTrigger(EnergyTrigger energyTrigger)
        => energyTrigger.Activate();

    private void DeactivateEnergyTrigger(EnergyTrigger energyTrigger)
        => energyTrigger.Deactivate();
}
